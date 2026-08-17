using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SoftwareContabil.Application.IService;

namespace SoftwareContabil.Api.Controllers;

public record LoginDto(
    [property: Required, EmailAddress] string Email,
    [property: Required] string Senha);

public record RegistrarUsuarioDto(
    [property: Required, EmailAddress] string Email,
    [property: Required, MinLength(6)] string Senha);

/// <summary>
/// Login e registro de usuário. Sujeito a rate limiting ("auth", 10
/// requisições/minuto por padrão — ver Program.cs) para dificultar ataques
/// de força bruta contra o login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginDto dto, CancellationToken ct)
        => Ok(await _authService.LoginAsync(new LoginRequest(dto.Email, dto.Senha), ct));

    [AllowAnonymous]
    [HttpPost("registrar")]
    public async Task<ActionResult> Registrar([FromBody] RegistrarUsuarioDto dto, CancellationToken ct)
    {
        var usuario = await _authService.RegistrarAsync(new RegistrarUsuarioRequest(dto.Email, dto.Senha), ct);
        return Ok(new { usuario.Id, usuario.Email });
    }
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record LoginRequest(string Email, string Senha);
public record LoginResult(string Token, string Email, DateTime ExpiraEm);
public record RegistrarUsuarioRequest(string Email, string Senha);

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Usuario> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default);
}

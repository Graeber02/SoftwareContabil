using Microsoft.Extensions.Logging;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

/// <summary>
/// Regras de autenticação. Depende apenas de abstrações
/// (<see cref="IUsuarioRepository"/>, <see cref="IPasswordHasher"/>,
/// <see cref="ITokenGenerator"/>) — trocar BCrypt por outro algoritmo, ou
/// JWT por outro esquema de token, não exige alterar esta classe.
/// </summary>
public class AuthService : IAuthService
{
    private const int SenhaTamanhoMinimo = 6;

    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email, ct);
        if (usuario is null || !_passwordHasher.Verify(request.Senha, usuario.Senha))
        {
            // Nunca logar a senha em texto claro — apenas o email tentado, para auditoria de acessos.
            _logger.LogWarning("Tentativa de login inválida para o email {Email}", request.Email);
            throw new BusinessRuleException("Email ou senha inválidos.");
        }

        var (token, expiraEm) = _tokenGenerator.GenerateToken(usuario.Id, usuario.Email);
        _logger.LogInformation("Login bem-sucedido: usuário {UsuarioId} ({Email})", usuario.Id, usuario.Email);
        return new LoginResult(token, usuario.Email, expiraEm);
    }

    public async Task<Usuario> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Senha) || request.Senha.Length < SenhaTamanhoMinimo)
            throw new BusinessRuleException($"A senha deve ter pelo menos {SenhaTamanhoMinimo} caracteres.");

        if (await _usuarioRepository.GetByEmailAsync(request.Email, ct) is not null)
            throw new BusinessRuleException("Já existe um usuário com este email.");

        var usuario = new Usuario
        {
            Email = request.Email,
            Senha = _passwordHasher.Hash(request.Senha),
        };
        var criado = await _usuarioRepository.AddAsync(usuario, ct);
        _logger.LogInformation("Novo usuário registrado: {UsuarioId} ({Email})", criado.Id, criado.Email);
        return criado;
    }
}

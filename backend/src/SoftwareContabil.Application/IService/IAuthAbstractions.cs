namespace SoftwareContabil.Application.IService;

/// <summary>Abstrai o algoritmo de hash de senha — a Application não conhece BCrypt.</summary>
public interface IPasswordHasher
{
    string Hash(string plainText);
    bool Verify(string plainText, string hash);
}

/// <summary>Abstrai a geração do token de acesso — a Application não conhece JWT.</summary>
public interface ITokenGenerator
{
    (string token, DateTime expiraEm) GenerateToken(int usuarioId, string email);
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftwareContabil.Application.IService;

namespace SoftwareContabil.Infrastructure.Security;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    public (string token, DateTime expiraEm) GenerateToken(int usuarioId, string email)
    {
        var jwtKey = _config["Jwt:Key"]!;
        var expiraEm = DateTime.UtcNow.AddHours(8);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiraEm,
            signingCredentials: creds);
        return (new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
    }
}

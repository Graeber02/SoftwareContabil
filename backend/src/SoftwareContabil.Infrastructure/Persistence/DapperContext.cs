using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SoftwareContabil.Infrastructure.Persistence;

/// <summary>
/// Contexto Dapper usado para consultas de leitura otimizadas e relatórios
/// (DRE, hierarquia do plano de contas, saldos), complementando o EF Core
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}

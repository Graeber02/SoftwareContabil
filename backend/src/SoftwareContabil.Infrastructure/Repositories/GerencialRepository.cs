using Dapper;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

/// <summary>
/// Implementação via Dapper das consultas gerenciais, chamando diretamente
/// as funções PL/pgSQL de negócio já existentes no banco (mantidas do
/// sistema original) — mais eficiente do que recalcular DRE/balancete em
/// memória a cada requisição.
/// </summary>
public class GerencialRepository : IGerencialRepository
{
    private readonly DapperContext _dapper;

    public GerencialRepository(DapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IReadOnlyList<DreLinha>> GetDreAsync(string cliforId, CancellationToken ct = default)
    {
        using var conn = _dapper.CreateConnection();
        var linhas = await conn.QueryAsync<DreLinha>(
            "select ordem, valortotal as ValorTotal, grupo from calcula_dre(@cliforId) order by ordem",
            new { cliforId });
        return linhas.ToList();
    }

    public async Task<IReadOnlyList<BalanceteLinha>> GetBalanceteAsync(string cliforId, CancellationToken ct = default)
    {
        const string sql = @"
            select c.id as Id,
                   c.descricao as Descricao,
                   coalesce(c.hierarquia, '') as Hierarquia,
                   coalesce(sum(l.valor), 0) as Saldo
            from conta c
            left join lancamentocontabil l on l.idconta = c.id
            where c.cliforid = @cliforId
            group by c.id, c.descricao, c.hierarquia
            order by c.ordem";
        using var conn = _dapper.CreateConnection();
        var linhas = await conn.QueryAsync<BalanceteLinha>(sql, new { cliforId });
        return linhas.ToList();
    }
}

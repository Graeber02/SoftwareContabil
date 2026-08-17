namespace SoftwareContabil.Domain.Repositories;

/// <summary>Linha de resultado do DRE (Demonstração do Resultado do Exercício).</summary>
public record DreLinha(int Ordem, decimal ValorTotal, string Grupo);

/// <summary>Linha de resultado do balancete de verificação.</summary>
public record BalanceteLinha(int Id, string Descricao, string Hierarquia, decimal Saldo);

/// <summary>
/// Consultas gerenciais que residem no banco (funções PL/pgSQL herdadas do
/// sistema original) e por isso são lidas via Dapper
/// </summary>
public interface IGerencialRepository
{
    Task<IReadOnlyList<DreLinha>> GetDreAsync(string cliforId, CancellationToken ct = default);
    Task<IReadOnlyList<BalanceteLinha>> GetBalanceteAsync(string cliforId, CancellationToken ct = default);
}

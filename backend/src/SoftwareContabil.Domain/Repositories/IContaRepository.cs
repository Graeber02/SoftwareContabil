using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IContaRepository
{
    Task<IReadOnlyList<Conta>> GetAllAsync(CancellationToken ct = default);
    Task<Conta?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Conta> AddAsync(Conta entity, CancellationToken ct = default);
    Task UpdateAsync(Conta entity, CancellationToken ct = default);
    Task DeleteAsync(Conta entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Conta>> GetByClienteAsync(string cliforId, CancellationToken ct = default);

    /// <summary>Ids de todas as contas "filhas" (na árvore) de uma conta sintética,
    /// identificada pela descrição — usado pelo motor do DRE.</summary>
    Task<IReadOnlyList<int>> GetContasFilhasIdsAsync(string cliforId, string descricao, CancellationToken ct = default);
}

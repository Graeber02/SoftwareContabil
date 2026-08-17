using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ILancamentoContabilRepository
{
    Task<IReadOnlyList<LancamentoContabil>> GetAllAsync(CancellationToken ct = default);
    Task<LancamentoContabil?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<LancamentoContabil> AddAsync(LancamentoContabil entity, CancellationToken ct = default);
    Task UpdateAsync(LancamentoContabil entity, CancellationToken ct = default);
    Task DeleteAsync(LancamentoContabil entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    /// <summary>Lista paginada — recomendado pois lançamentos contábeis crescem indefinidamente.</summary>
    Task<IReadOnlyList<LancamentoContabil>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

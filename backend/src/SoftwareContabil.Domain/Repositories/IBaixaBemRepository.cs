using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IBaixaBemRepository
{
    Task<IReadOnlyList<BaixaBem>> GetAllAsync(CancellationToken ct = default);
    Task<BaixaBem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BaixaBem> AddAsync(BaixaBem entity, CancellationToken ct = default);
    Task UpdateAsync(BaixaBem entity, CancellationToken ct = default);
    Task DeleteAsync(BaixaBem entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

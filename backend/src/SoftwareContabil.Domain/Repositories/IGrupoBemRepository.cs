using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IGrupoBemRepository
{
    Task<IReadOnlyList<GrupoBem>> GetAllAsync(CancellationToken ct = default);
    Task<GrupoBem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<GrupoBem> AddAsync(GrupoBem entity, CancellationToken ct = default);
    Task UpdateAsync(GrupoBem entity, CancellationToken ct = default);
    Task DeleteAsync(GrupoBem entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

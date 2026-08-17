using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IDepreciacaoRepository
{
    Task<IReadOnlyList<Depreciacao>> GetAllAsync(CancellationToken ct = default);
    Task<Depreciacao?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Depreciacao> AddAsync(Depreciacao entity, CancellationToken ct = default);
    Task UpdateAsync(Depreciacao entity, CancellationToken ct = default);
    Task DeleteAsync(Depreciacao entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

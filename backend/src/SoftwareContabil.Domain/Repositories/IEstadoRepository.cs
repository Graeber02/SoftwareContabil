using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IEstadoRepository
{
    Task<IReadOnlyList<Estado>> GetAllAsync(CancellationToken ct = default);
    Task<Estado?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Estado> AddAsync(Estado entity, CancellationToken ct = default);
    Task UpdateAsync(Estado entity, CancellationToken ct = default);
    Task DeleteAsync(Estado entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

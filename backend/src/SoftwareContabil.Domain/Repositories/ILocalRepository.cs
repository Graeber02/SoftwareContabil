using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ILocalRepository
{
    Task<IReadOnlyList<Local>> GetAllAsync(CancellationToken ct = default);
    Task<Local?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Local> AddAsync(Local entity, CancellationToken ct = default);
    Task UpdateAsync(Local entity, CancellationToken ct = default);
    Task DeleteAsync(Local entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

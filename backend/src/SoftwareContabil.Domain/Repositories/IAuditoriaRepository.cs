using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IAuditoriaRepository
{
    Task<IReadOnlyList<Auditoria>> GetAllAsync(CancellationToken ct = default);
    Task<Auditoria?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Auditoria> AddAsync(Auditoria entity, CancellationToken ct = default);
    Task UpdateAsync(Auditoria entity, CancellationToken ct = default);
    Task DeleteAsync(Auditoria entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

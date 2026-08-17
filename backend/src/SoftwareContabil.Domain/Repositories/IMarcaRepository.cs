using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IMarcaRepository
{
    Task<IReadOnlyList<Marca>> GetAllAsync(CancellationToken ct = default);
    Task<Marca?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Marca> AddAsync(Marca entity, CancellationToken ct = default);
    Task UpdateAsync(Marca entity, CancellationToken ct = default);
    Task DeleteAsync(Marca entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IEspecieRepository
{
    Task<IReadOnlyList<Especie>> GetAllAsync(CancellationToken ct = default);
    Task<Especie?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Especie> AddAsync(Especie entity, CancellationToken ct = default);
    Task UpdateAsync(Especie entity, CancellationToken ct = default);
    Task DeleteAsync(Especie entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

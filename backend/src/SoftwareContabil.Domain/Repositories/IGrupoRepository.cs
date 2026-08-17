using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IGrupoRepository
{
    Task<IReadOnlyList<Grupo>> GetAllAsync(CancellationToken ct = default);
    Task<Grupo?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Grupo> AddAsync(Grupo entity, CancellationToken ct = default);
    Task UpdateAsync(Grupo entity, CancellationToken ct = default);
    Task DeleteAsync(Grupo entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

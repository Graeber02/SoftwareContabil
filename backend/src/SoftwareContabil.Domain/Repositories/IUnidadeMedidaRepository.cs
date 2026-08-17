using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IUnidadeMedidaRepository
{
    Task<IReadOnlyList<UnidadeMedida>> GetAllAsync(CancellationToken ct = default);
    Task<UnidadeMedida?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<UnidadeMedida> AddAsync(UnidadeMedida entity, CancellationToken ct = default);
    Task UpdateAsync(UnidadeMedida entity, CancellationToken ct = default);
    Task DeleteAsync(UnidadeMedida entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IFechaEstoqueRepository
{
    Task<IReadOnlyList<FechaEstoque>> GetAllAsync(CancellationToken ct = default);
    Task<FechaEstoque?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<FechaEstoque> AddAsync(FechaEstoque entity, CancellationToken ct = default);
    Task UpdateAsync(FechaEstoque entity, CancellationToken ct = default);
    Task DeleteAsync(FechaEstoque entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

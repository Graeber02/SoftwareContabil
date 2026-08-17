using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ICentroCustoRepository
{
    Task<IReadOnlyList<CentroCusto>> GetAllAsync(CancellationToken ct = default);
    Task<CentroCusto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CentroCusto> AddAsync(CentroCusto entity, CancellationToken ct = default);
    Task UpdateAsync(CentroCusto entity, CancellationToken ct = default);
    Task DeleteAsync(CentroCusto entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService
{
    public interface ICentroCustoService
    {
        Task<IReadOnlyList<CentroCusto>> GetAllAsync(CancellationToken ct = default);
        Task<CentroCusto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CentroCusto> CreateAsync(CentroCusto entity, CancellationToken ct = default);
        Task UpdateAsync(int id, CentroCusto entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

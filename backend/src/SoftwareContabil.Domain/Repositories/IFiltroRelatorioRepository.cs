using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IFiltroRelatorioRepository
{
    Task<IReadOnlyList<FiltroRelatorio>> GetAllAsync(CancellationToken ct = default);
    Task<FiltroRelatorio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<FiltroRelatorio> AddAsync(FiltroRelatorio entity, CancellationToken ct = default);
    Task UpdateAsync(FiltroRelatorio entity, CancellationToken ct = default);
    Task DeleteAsync(FiltroRelatorio entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

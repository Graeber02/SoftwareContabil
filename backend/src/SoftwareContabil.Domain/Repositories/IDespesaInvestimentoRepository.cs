using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IDespesaInvestimentoRepository
{
    Task<IReadOnlyList<DespesaInvestimento>> GetAllAsync(CancellationToken ct = default);
    Task<DespesaInvestimento?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<DespesaInvestimento> AddAsync(DespesaInvestimento entity, CancellationToken ct = default);
    Task UpdateAsync(DespesaInvestimento entity, CancellationToken ct = default);
    Task DeleteAsync(DespesaInvestimento entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

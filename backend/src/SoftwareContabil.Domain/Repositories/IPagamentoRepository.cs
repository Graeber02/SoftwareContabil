using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IPagamentoRepository
{
    Task<IReadOnlyList<Pagamento>> GetAllAsync(CancellationToken ct = default);
    Task<Pagamento?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Pagamento> AddAsync(Pagamento entity, CancellationToken ct = default);
    Task UpdateAsync(Pagamento entity, CancellationToken ct = default);
    Task DeleteAsync(Pagamento entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

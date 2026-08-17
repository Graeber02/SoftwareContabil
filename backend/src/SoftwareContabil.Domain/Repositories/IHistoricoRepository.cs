using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IHistoricoRepository
{
    Task<IReadOnlyList<Historico>> GetAllAsync(CancellationToken ct = default);
    Task<Historico?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Historico> AddAsync(Historico entity, CancellationToken ct = default);
    Task UpdateAsync(Historico entity, CancellationToken ct = default);
    Task DeleteAsync(Historico entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

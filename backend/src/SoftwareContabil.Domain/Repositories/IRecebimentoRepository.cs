using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IRecebimentoRepository
{
    Task<IReadOnlyList<Recebimento>> GetAllAsync(CancellationToken ct = default);
    Task<Recebimento?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Recebimento> AddAsync(Recebimento entity, CancellationToken ct = default);
    Task UpdateAsync(Recebimento entity, CancellationToken ct = default);
    Task DeleteAsync(Recebimento entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

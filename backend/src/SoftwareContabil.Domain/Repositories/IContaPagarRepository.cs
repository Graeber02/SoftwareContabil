using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IContaPagarRepository
{
    Task<IReadOnlyList<ContaPagar>> GetAllAsync(CancellationToken ct = default);
    Task<ContaPagar?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaPagar> AddAsync(ContaPagar entity, CancellationToken ct = default);
    Task UpdateAsync(ContaPagar entity, CancellationToken ct = default);
    Task DeleteAsync(ContaPagar entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ContaPagar>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default);
}

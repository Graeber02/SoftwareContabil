using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IContaReceberRepository
{
    Task<IReadOnlyList<ContaReceber>> GetAllAsync(CancellationToken ct = default);
    Task<ContaReceber?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaReceber> AddAsync(ContaReceber entity, CancellationToken ct = default);
    Task UpdateAsync(ContaReceber entity, CancellationToken ct = default);
    Task DeleteAsync(ContaReceber entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ContaReceber>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default);
}

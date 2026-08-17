using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IContaCorrenteRepository
{
    Task<IReadOnlyList<ContaCorrente>> GetAllAsync(CancellationToken ct = default);
    Task<ContaCorrente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaCorrente> AddAsync(ContaCorrente entity, CancellationToken ct = default);
    Task UpdateAsync(ContaCorrente entity, CancellationToken ct = default);
    Task DeleteAsync(ContaCorrente entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

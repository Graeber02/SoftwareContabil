using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IPrecoRepository
{
    Task<IReadOnlyList<Preco>> GetAllAsync(CancellationToken ct = default);
    Task<Preco?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Preco> AddAsync(Preco entity, CancellationToken ct = default);
    Task UpdateAsync(Preco entity, CancellationToken ct = default);
    Task DeleteAsync(Preco entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

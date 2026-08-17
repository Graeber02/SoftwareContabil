using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ICidadeRepository
{
    Task<IReadOnlyList<Cidade>> GetAllAsync(CancellationToken ct = default);
    Task<Cidade?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Cidade> AddAsync(Cidade entity, CancellationToken ct = default);
    Task UpdateAsync(Cidade entity, CancellationToken ct = default);
    Task DeleteAsync(Cidade entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

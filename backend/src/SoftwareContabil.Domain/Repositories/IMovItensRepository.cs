using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IMovItensRepository
{
    Task<IReadOnlyList<MovItens>> GetAllAsync(CancellationToken ct = default);
    Task<MovItens?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<MovItens> AddAsync(MovItens entity, CancellationToken ct = default);
    Task UpdateAsync(MovItens entity, CancellationToken ct = default);
    Task DeleteAsync(MovItens entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

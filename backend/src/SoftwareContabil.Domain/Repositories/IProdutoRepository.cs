using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IProdutoRepository
{
    Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct = default);
    Task<Produto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Produto> AddAsync(Produto entity, CancellationToken ct = default);
    Task UpdateAsync(Produto entity, CancellationToken ct = default);
    Task DeleteAsync(Produto entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    /// <summary>Lista paginada — recomendado para o catálogo de produtos quando crescer muito.</summary>
    Task<IReadOnlyList<Produto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

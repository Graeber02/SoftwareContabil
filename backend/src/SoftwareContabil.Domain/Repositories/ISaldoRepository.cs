using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ISaldoRepository
{
    Task<IReadOnlyList<Saldo>> GetAllAsync(CancellationToken ct = default);
    Task<Saldo?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Saldo> AddAsync(Saldo entity, CancellationToken ct = default);
    Task UpdateAsync(Saldo entity, CancellationToken ct = default);
    Task DeleteAsync(Saldo entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    Task<Saldo?> GetByProdutoLocalAsync(int produtoId, int localId, CancellationToken ct = default);
}

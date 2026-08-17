using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IRelatorioRepository
{
    Task<IReadOnlyList<Relatorio>> GetAllAsync(CancellationToken ct = default);
    Task<Relatorio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Relatorio> AddAsync(Relatorio entity, CancellationToken ct = default);
    Task UpdateAsync(Relatorio entity, CancellationToken ct = default);
    Task DeleteAsync(Relatorio entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

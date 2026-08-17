using SoftwareContabil.Domain.Entities;

public interface IBaixaBemService
{
    Task<IReadOnlyList<BaixaBem>> GetAllAsync(CancellationToken ct = default);
    Task<BaixaBem> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BaixaBem> CreateAsync(BaixaBem entity, CancellationToken ct = default);
    Task UpdateAsync(int id, BaixaBem entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IPatrimonioRepository
{
    Task<IReadOnlyList<Patrimonio>> GetAllAsync(CancellationToken ct = default);
    Task<Patrimonio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Patrimonio> AddAsync(Patrimonio entity, CancellationToken ct = default);
    Task UpdateAsync(Patrimonio entity, CancellationToken ct = default);
    Task DeleteAsync(Patrimonio entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Patrimonio>> GetAtivosAsync(string cliforId, CancellationToken ct = default);
}

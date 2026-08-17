using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IMotivoBaixaRepository
{
    Task<IReadOnlyList<MotivoBaixa>> GetAllAsync(CancellationToken ct = default);
    Task<MotivoBaixa?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<MotivoBaixa> AddAsync(MotivoBaixa entity, CancellationToken ct = default);
    Task UpdateAsync(MotivoBaixa entity, CancellationToken ct = default);
    Task DeleteAsync(MotivoBaixa entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

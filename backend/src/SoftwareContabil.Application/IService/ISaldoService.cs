using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public interface ISaldoService
{
    Task<IReadOnlyList<Saldo>> GetAllAsync(CancellationToken ct = default);
    Task<Saldo> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Saldo> CreateAsync(Saldo entity, CancellationToken ct = default);
    Task UpdateAsync(int id, Saldo entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

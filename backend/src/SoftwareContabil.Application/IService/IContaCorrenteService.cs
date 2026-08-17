using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService
{
    public interface IContaCorrenteService
    {
        Task<IReadOnlyList<ContaCorrente>> GetAllAsync(CancellationToken ct = default);
        Task<ContaCorrente> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ContaCorrente> CreateAsync(ContaCorrente entity, CancellationToken ct = default);
        Task UpdateAsync(int id, ContaCorrente entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

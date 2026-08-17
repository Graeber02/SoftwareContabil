using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService
{
    public interface ICidadeService
    {
        Task<IReadOnlyList<Cidade>> GetAllAsync(CancellationToken ct = default);
        Task<Cidade> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Cidade> CreateAsync(Cidade entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Cidade entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

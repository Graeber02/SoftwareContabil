using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IEnderecoRepository
{
    Task<IReadOnlyList<Endereco>> GetAllAsync(CancellationToken ct = default);
    Task<Endereco?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Endereco> AddAsync(Endereco entity, CancellationToken ct = default);
    Task UpdateAsync(Endereco entity, CancellationToken ct = default);
    Task DeleteAsync(Endereco entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

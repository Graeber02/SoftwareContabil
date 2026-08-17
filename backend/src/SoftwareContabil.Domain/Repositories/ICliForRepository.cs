using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ICliForRepository
{
    Task<IReadOnlyList<CliFor>> GetAllAsync(CancellationToken ct = default);
    Task<CliFor?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<CliFor> AddAsync(CliFor entity, CancellationToken ct = default);
    Task UpdateAsync(CliFor entity, CancellationToken ct = default);
    Task DeleteAsync(CliFor entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(string id, CancellationToken ct = default);

    /// <summary>Lista paginada — usada quando o cadastro cresce muito para trazer tudo de uma vez.</summary>
    Task<IReadOnlyList<CliFor>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

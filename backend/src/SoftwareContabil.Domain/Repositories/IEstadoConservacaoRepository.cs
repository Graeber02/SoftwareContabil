using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IEstadoConservacaoRepository
{
    Task<IReadOnlyList<EstadoConservacao>> GetAllAsync(CancellationToken ct = default);
    Task<EstadoConservacao?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<EstadoConservacao> AddAsync(EstadoConservacao entity, CancellationToken ct = default);
    Task UpdateAsync(EstadoConservacao entity, CancellationToken ct = default);
    Task DeleteAsync(EstadoConservacao entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

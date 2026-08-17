using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface ISolicitacaoRelatorioRepository
{
    Task<IReadOnlyList<SolicitacaoRelatorio>> GetAllAsync(CancellationToken ct = default);
    Task<SolicitacaoRelatorio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<SolicitacaoRelatorio> AddAsync(SolicitacaoRelatorio entity, CancellationToken ct = default);
    Task UpdateAsync(SolicitacaoRelatorio entity, CancellationToken ct = default);
    Task DeleteAsync(SolicitacaoRelatorio entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}

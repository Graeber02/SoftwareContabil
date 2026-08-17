using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record BaixarBemRequest(DateTime Data, double Valor, string? Observacao, int MotivoBaixaId);

public interface IPatrimonioService
{
    Task<IReadOnlyList<Domain.Entities.Patrimonio>> GetAllAsync(CancellationToken ct = default);
    Task<Domain.Entities.Patrimonio> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Domain.Entities.Patrimonio> CreateAsync(Domain.Entities.Patrimonio entity, CancellationToken ct = default);
    Task UpdateAsync(int id, Domain.Entities.Patrimonio entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Domain.Entities.Patrimonio>> GetAtivosAsync(string cliforId, CancellationToken ct = default);
    Task<BaixaBem> BaixarAsync(int patrimonioId, BaixarBemRequest request, CancellationToken ct = default);
}

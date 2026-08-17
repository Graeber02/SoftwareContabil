using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.IService;

public record ContasPagarReceberResumo(IReadOnlyList<ContaPagar> Pagar, IReadOnlyList<ContaReceber> Receber);

public interface IGerencialService
{
    Task<IReadOnlyList<DreLinha>> GetDreAsync(string cliforId, CancellationToken ct = default);
    Task<IReadOnlyList<BalanceteLinha>> GetBalanceteAsync(string cliforId, CancellationToken ct = default);
    Task<ContasPagarReceberResumo> GetContasPagarReceberAsync(string cliforId, CancellationToken ct = default);
}

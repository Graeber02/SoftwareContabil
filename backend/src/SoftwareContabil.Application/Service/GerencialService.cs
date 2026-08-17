using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class GerencialService : IGerencialService
{
    private readonly IGerencialRepository _gerencialRepository;
    private readonly IContaPagarRepository _contaPagarRepository;
    private readonly IContaReceberRepository _contaReceberRepository;

    public GerencialService(
        IGerencialRepository gerencialRepository,
        IContaPagarRepository contaPagarRepository,
        IContaReceberRepository contaReceberRepository)
    {
        _gerencialRepository = gerencialRepository;
        _contaPagarRepository = contaPagarRepository;
        _contaReceberRepository = contaReceberRepository;
    }

    public Task<IReadOnlyList<DreLinha>> GetDreAsync(string cliforId, CancellationToken ct = default)
        => _gerencialRepository.GetDreAsync(cliforId, ct);

    public Task<IReadOnlyList<BalanceteLinha>> GetBalanceteAsync(string cliforId, CancellationToken ct = default)
        => _gerencialRepository.GetBalanceteAsync(cliforId, ct);

    public async Task<ContasPagarReceberResumo> GetContasPagarReceberAsync(string cliforId, CancellationToken ct = default)
    {
        var pagar = await _contaPagarRepository.GetEmAbertoAsync(cliforId, ct);
        var receber = await _contaReceberRepository.GetEmAbertoAsync(cliforId, ct);
        return new ContasPagarReceberResumo(pagar, receber);
    }
}

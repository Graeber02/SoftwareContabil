using Microsoft.Extensions.Logging;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class ContaPagarService : IContaPagarService
{
    private readonly IContaPagarRepository _contaPagarRepository;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly ILogger<ContaPagarService> _logger;

    public ContaPagarService(
        IContaPagarRepository contaPagarRepository,
        IPagamentoRepository pagamentoRepository,
        ILogger<ContaPagarService> logger)
    {
        _contaPagarRepository = contaPagarRepository;
        _pagamentoRepository = pagamentoRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<ContaPagar>> GetAllAsync(CancellationToken ct = default)
        => _contaPagarRepository.GetAllAsync(ct);

    public async Task<ContaPagar> GetByIdAsync(int id, CancellationToken ct = default)
        => await _contaPagarRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(ContaPagar), id);

    public Task<ContaPagar> CreateAsync(ContaPagar entity, CancellationToken ct = default)
    {
        // Validação de negócio (além das Data Annotations já checadas pelo model binding):
        // vencimento não pode ser anterior à emissão do documento.
        if (entity.Datavencimento < entity.Datadocumento)
            throw new BusinessRuleException("A data de vencimento não pode ser anterior à data do documento.");

        // Ao criar, o saldo em aberto começa igual ao valor do título — o cliente
        // não deveria poder criar um título já parcialmente pago por fora do fluxo de pagamento.
        entity.Saldo = entity.Valor;
        entity.Datapagamento = null;

        return _contaPagarRepository.AddAsync(entity, ct);
    }

    public async Task UpdateAsync(int id, ContaPagar entity, CancellationToken ct = default)
    {
        if (entity.Id != id)
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _contaPagarRepository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(ContaPagar), id);
        if (entity.Datavencimento < entity.Datadocumento)
            throw new BusinessRuleException("A data de vencimento não pode ser anterior à data do documento.");

        await _contaPagarRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _contaPagarRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(ContaPagar), id);
        await _contaPagarRepository.DeleteAsync(entity, ct);
    }

    public Task<IReadOnlyList<ContaPagar>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default)
        => _contaPagarRepository.GetEmAbertoAsync(cliforId, ct);

    public async Task<Pagamento> PagarAsync(int contaPagarId, RegistrarPagamentoRequest request, CancellationToken ct = default)
    {
        var conta = await _contaPagarRepository.GetByIdAsync(contaPagarId, ct)
            ?? throw new NotFoundException(nameof(ContaPagar), contaPagarId);

        if (request.ValorPago <= 0)
            throw new BusinessRuleException("Valor pago deve ser maior que zero.");
        if (request.ValorPago > conta.Saldo)
            throw new BusinessRuleException("Valor pago não pode ser maior que o saldo do título.");

        var pagamento = new Pagamento
        {
            Valorpago = request.ValorPago,
            Datapagamento = request.DataPagamento,
            Descricao = request.Descricao ?? conta.Descricao,
            Contapagarid = conta.Id,
            Especieid = request.EspecieId,
            Contacorrenteid = request.ContaCorrenteId,
            Cliforid = conta.Cliforid,
        };

        conta.Saldo -= request.ValorPago;
        if (conta.Saldo <= 0)
        {
            conta.Saldo = 0;
            conta.Datapagamento = request.DataPagamento;
        }

        await _pagamentoRepository.AddAsync(pagamento, ct);
        await _contaPagarRepository.UpdateAsync(conta, ct);

        _logger.LogInformation(
            "Pagamento de {Valor:C} registrado para o título {ContaPagarId} (saldo restante: {Saldo:C})",
            request.ValorPago, contaPagarId, conta.Saldo);

        return pagamento;
    }
}

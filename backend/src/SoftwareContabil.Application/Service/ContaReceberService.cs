using Microsoft.Extensions.Logging;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class ContaReceberService : IContaReceberService
{
    private readonly IContaReceberRepository _contaReceberRepository;
    private readonly IRecebimentoRepository _recebimentoRepository;
    private readonly ILogger<ContaReceberService> _logger;

    public ContaReceberService(
        IContaReceberRepository contaReceberRepository,
        IRecebimentoRepository recebimentoRepository,
        ILogger<ContaReceberService> logger)
    {
        _contaReceberRepository = contaReceberRepository;
        _recebimentoRepository = recebimentoRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<ContaReceber>> GetAllAsync(CancellationToken ct = default)
        => _contaReceberRepository.GetAllAsync(ct);

    public async Task<ContaReceber> GetByIdAsync(int id, CancellationToken ct = default)
        => await _contaReceberRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(ContaReceber), id);

    public Task<ContaReceber> CreateAsync(ContaReceber entity, CancellationToken ct = default)
    {
        if (entity.Datavencimento < entity.Datadocumento)
            throw new BusinessRuleException("A data de vencimento não pode ser anterior à data do documento.");

        entity.Saldo = entity.Valor;
        entity.Datarecebimento = null;

        return _contaReceberRepository.AddAsync(entity, ct);
    }

    public async Task UpdateAsync(int id, ContaReceber entity, CancellationToken ct = default)
    {
        if (entity.Id != id)
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _contaReceberRepository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(ContaReceber), id);
        if (entity.Datavencimento < entity.Datadocumento)
            throw new BusinessRuleException("A data de vencimento não pode ser anterior à data do documento.");

        await _contaReceberRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _contaReceberRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(ContaReceber), id);
        await _contaReceberRepository.DeleteAsync(entity, ct);
    }

    public Task<IReadOnlyList<ContaReceber>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default)
        => _contaReceberRepository.GetEmAbertoAsync(cliforId, ct);

    public async Task<Recebimento> ReceberAsync(int contaReceberId, RegistrarRecebimentoRequest request, CancellationToken ct = default)
    {
        var conta = await _contaReceberRepository.GetByIdAsync(contaReceberId, ct)
            ?? throw new NotFoundException(nameof(ContaReceber), contaReceberId);

        if (request.ValorRecebido <= 0)
            throw new BusinessRuleException("Valor recebido deve ser maior que zero.");
        if (request.ValorRecebido > conta.Saldo)
            throw new BusinessRuleException("Valor recebido não pode ser maior que o saldo do título.");

        var recebimento = new Recebimento
        {
            Valorrecebido = request.ValorRecebido,
            Datarecebimento = request.DataRecebimento,
            Descricao = request.Descricao ?? conta.Descricao,
            Contareceberid = conta.Id,
            Especieid = request.EspecieId,
            Contacorrenteid = request.ContaCorrenteId,
            Cliforid = conta.Cliforid,
        };

        conta.Saldo -= request.ValorRecebido;
        if (conta.Saldo <= 0)
        {
            conta.Saldo = 0;
            conta.Datarecebimento = request.DataRecebimento;
        }

        await _recebimentoRepository.AddAsync(recebimento, ct);
        await _contaReceberRepository.UpdateAsync(conta, ct);

        _logger.LogInformation(
            "Recebimento de {Valor:C} registrado para o título {ContaReceberId} (saldo restante: {Saldo:C})",
            request.ValorRecebido, contaReceberId, conta.Saldo);

        return recebimento;
    }
}

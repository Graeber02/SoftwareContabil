using Microsoft.Extensions.Logging;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class MovimentacaoService : IMovimentacaoService
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly ILogger<MovimentacaoService> _logger;

    public MovimentacaoService(IMovimentacaoRepository movimentacaoRepository, ILogger<MovimentacaoService> logger)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<Movimentacao>> GetAllAsync(CancellationToken ct = default)
        => _movimentacaoRepository.GetAllAsync(ct);

    public async Task<Movimentacao> GetByIdAsync(int id, CancellationToken ct = default)
        => await _movimentacaoRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Movimentacao), id);

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _movimentacaoRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Movimentacao), id);
        await _movimentacaoRepository.DeleteAsync(entity, ct);
    }

    public async Task<Movimentacao> CriarCompletaAsync(CriarMovimentacaoRequest request, CancellationToken ct = default)
    {
        if (request.Itens.Count == 0)
            throw new BusinessRuleException("Informe ao menos um item.");
        if (request.Tipo is not ('C' or 'c' or 'V' or 'v'))
            throw new BusinessRuleException("Tipo de movimentação inválido: use 'C' para compra ou 'V' para venda.");

        foreach (var item in request.Itens)
        {
            if (item.Quantidade <= 0)
                throw new BusinessRuleException($"Quantidade do produto {item.ProdutoId} deve ser maior que zero.");
            if (item.Valor < 0)
                throw new BusinessRuleException($"Valor do produto {item.ProdutoId} não pode ser negativo.");
        }

        var movimentacao = new Movimentacao
        {
            Notafiscal = request.NotaFiscal,
            Tipo = request.Tipo,
            Data = request.Data,
            Valortotal = request.Itens.Sum(i => i.Quantidade * i.Valor),
            Cliforid = request.CliForId,
            Empresaid = request.EmpresaId,
        };

        var sequencia = 1;
        var itens = request.Itens.Select(i => new MovItens
        {
            Sequencia = sequencia++,
            Produtoid = i.ProdutoId,
            Localid = i.LocalId,
            Quantidade = i.Quantidade,
            Valor = i.Valor,
            Cliforid = request.CliForId,
        }).ToList();

        var criada = await _movimentacaoRepository.CriarComItensAsync(movimentacao, itens, ct);

        _logger.LogInformation(
            "{Tipo} registrada: movimentação {MovimentacaoId}, {QtdItens} item(ns), total {Total:C}",
            request.Tipo is 'C' or 'c' ? "Compra" : "Venda", criada.Id, itens.Count, criada.Valortotal);

        return criada;
    }

    public Task<IReadOnlyList<MovItens>> GetItensAsync(int movimentacaoId, CancellationToken ct = default)
        => _movimentacaoRepository.GetItensAsync(movimentacaoId, ct);

    public Task<IReadOnlyList<Movimentacao>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;
        return _movimentacaoRepository.GetPagedAsync(page, pageSize, ct);
    }
}

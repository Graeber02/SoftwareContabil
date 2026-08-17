using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class ContaService : IContaService
{
    private readonly IContaRepository _contaRepository;

    public ContaService(IContaRepository contaRepository)
    {
        _contaRepository = contaRepository;
    }

    public Task<IReadOnlyList<Conta>> GetAllAsync(CancellationToken ct = default)
        => _contaRepository.GetAllAsync(ct);

    public async Task<Conta> GetByIdAsync(int id, CancellationToken ct = default)
        => await _contaRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Conta), id);

    public Task<Conta> CreateAsync(Conta entity, CancellationToken ct = default)
        => _contaRepository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, Conta entity, CancellationToken ct = default)
    {
        if (entity.Id != id)
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _contaRepository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(Conta), id);
        await _contaRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _contaRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Conta), id);
        await _contaRepository.DeleteAsync(entity, ct);
    }

    public async Task<IReadOnlyList<ContaTreeNode>> GetArvoreAsync(string cliforId, CancellationToken ct = default)
    {
        var contas = await _contaRepository.GetByClienteAsync(cliforId, ct);

        var nodes = contas
            .OrderBy(c => c.Ordem)
            .ToDictionary(c => c.Id, c => new ContaTreeNode(c.Id, c.Descricao, c.Sintetica, c.Ordem, c.Contapai));

        var roots = new List<ContaTreeNode>();
        foreach (var node in nodes.Values)
        {
            if (node.ContaPaiId is int paiId && nodes.TryGetValue(paiId, out var pai))
                pai.Filhos.Add(node);
            else
                roots.Add(node);
        }
        return roots;
    }

    public Task<IReadOnlyList<int>> GetContasFilhasAsync(string cliforId, string descricao, CancellationToken ct = default)
        => _contaRepository.GetContasFilhasIdsAsync(cliforId, descricao, ct);
}

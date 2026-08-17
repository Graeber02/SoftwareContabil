using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class DespesaInvestimentoService : IDespesaInvestimentoService
{
    private readonly IDespesaInvestimentoRepository _repository;

    public DespesaInvestimentoService(IDespesaInvestimentoRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<DespesaInvestimento>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<DespesaInvestimento> GetByIdAsync(int id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(DespesaInvestimento), id!);

    public Task<DespesaInvestimento> CreateAsync(DespesaInvestimento entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, DespesaInvestimento entity, CancellationToken ct = default)
    {
        if (!Equals(entity.Id, id))
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(DespesaInvestimento), id!);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(DespesaInvestimento), id!);
        await _repository.DeleteAsync(entity, ct);
    }
}

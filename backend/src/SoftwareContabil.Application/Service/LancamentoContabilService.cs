using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class LancamentoContabilService : ILancamentoContabilService
{
    private readonly ILancamentoContabilRepository _repository;

    public LancamentoContabilService(ILancamentoContabilRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<LancamentoContabil>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<LancamentoContabil> GetByIdAsync(int id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(LancamentoContabil), id!);

    public Task<LancamentoContabil> CreateAsync(LancamentoContabil entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, LancamentoContabil entity, CancellationToken ct = default)
    {
        if (!Equals(entity.Id, id))
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(LancamentoContabil), id!);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(LancamentoContabil), id!);
        await _repository.DeleteAsync(entity, ct);
    }

    public Task<IReadOnlyList<LancamentoContabil>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;
        return _repository.GetPagedAsync(page, pageSize, ct);
    }
}

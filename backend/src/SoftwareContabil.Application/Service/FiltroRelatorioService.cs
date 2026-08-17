using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class FiltroRelatorioService : IFiltroRelatorioService
{
    private readonly IFiltroRelatorioRepository _repository;

    public FiltroRelatorioService(IFiltroRelatorioRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<FiltroRelatorio>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<FiltroRelatorio> GetByIdAsync(int id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(FiltroRelatorio), id!);

    public Task<FiltroRelatorio> CreateAsync(FiltroRelatorio entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, FiltroRelatorio entity, CancellationToken ct = default)
    {
        if (!Equals(entity.Id, id))
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(FiltroRelatorio), id!);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(FiltroRelatorio), id!);
        await _repository.DeleteAsync(entity, ct);
    }
}

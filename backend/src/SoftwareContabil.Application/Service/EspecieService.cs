using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;



public class EspecieService : IEspecieService
{
    private readonly IEspecieRepository _repository;

    public EspecieService(IEspecieRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<Especie>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<Especie> GetByIdAsync(int id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Especie), id!);

    public Task<Especie> CreateAsync(Especie entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, Especie entity, CancellationToken ct = default)
    {
        if (!Equals(entity.Id, id))
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(Especie), id!);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Especie), id!);
        await _repository.DeleteAsync(entity, ct);
    }
}

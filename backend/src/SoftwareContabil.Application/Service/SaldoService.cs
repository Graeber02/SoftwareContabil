using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class SaldoService : ISaldoService
{
    private readonly ISaldoRepository _repository;

    public SaldoService(ISaldoRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<Saldo>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<Saldo> GetByIdAsync(int id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Saldo), id);

    public Task<Saldo> CreateAsync(Saldo entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(int id, Saldo entity, CancellationToken ct = default)
    {
        if (entity.Id != id)
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(Saldo), id);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Saldo), id);
        await _repository.DeleteAsync(entity, ct);
    }
}

using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class CliForService : ICliForService
{
    private readonly ICliForRepository _repository;

    public CliForService(ICliForRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<CliFor>> GetAllAsync(CancellationToken ct = default)
        => _repository.GetAllAsync(ct);

    public async Task<CliFor> GetByIdAsync(string id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(CliFor), id!);

    public Task<CliFor> CreateAsync(CliFor entity, CancellationToken ct = default)
        => _repository.AddAsync(entity, ct);

    public async Task UpdateAsync(string id, CliFor entity, CancellationToken ct = default)
    {
        if (!Equals(entity.Id, id))
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _repository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(CliFor), id!);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(CliFor), id!);
        await _repository.DeleteAsync(entity, ct);
    }

    public Task<IReadOnlyList<CliFor>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;
        return _repository.GetPagedAsync(page, pageSize, ct);
    }
}

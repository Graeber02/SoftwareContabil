using Microsoft.Extensions.Logging;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Exceptions;
using SoftwareContabil.Domain.Repositories;

namespace SoftwareContabil.Application.Service;

public class PatrimonioService : IPatrimonioService
{
    private readonly IPatrimonioRepository _patrimonioRepository;
    private readonly IBaixaBemRepository _baixaBemRepository;
    private readonly ILogger<PatrimonioService> _logger;

    public PatrimonioService(
        IPatrimonioRepository patrimonioRepository,
        IBaixaBemRepository baixaBemRepository,
        ILogger<PatrimonioService> logger)
    {
        _patrimonioRepository = patrimonioRepository;
        _baixaBemRepository = baixaBemRepository;
        _logger = logger;
    }

    public Task<IReadOnlyList<Patrimonio>> GetAllAsync(CancellationToken ct = default)
        => _patrimonioRepository.GetAllAsync(ct);

    public async Task<Patrimonio> GetByIdAsync(int id, CancellationToken ct = default)
        => await _patrimonioRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Domain.Entities.Patrimonio), id);

    public Task<Patrimonio> CreateAsync(Patrimonio entity, CancellationToken ct = default)
    {
        // Um bem nasce sempre ativo — "baixado" só muda através da ação BaixarAsync,
        // nunca diretamente pelo cliente na criação.
        entity.Baixado = 0;
        return _patrimonioRepository.AddAsync(entity, ct);
    }

    public async Task UpdateAsync(int id, Patrimonio entity, CancellationToken ct = default)
    {
        if (entity.Id != id)
            throw new BusinessRuleException("O Id do corpo da requisição difere do Id da rota.");
        if (!await _patrimonioRepository.ExistsAsync(id, ct))
            throw new NotFoundException(nameof(Domain.Entities.Patrimonio), id);
        await _patrimonioRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _patrimonioRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Domain.Entities.Patrimonio), id);
        await _patrimonioRepository.DeleteAsync(entity, ct);
    }

    public Task<IReadOnlyList<Patrimonio>> GetAtivosAsync(string cliforId, CancellationToken ct = default)
        => _patrimonioRepository.GetAtivosAsync(cliforId, ct);

    public async Task<BaixaBem> BaixarAsync(int patrimonioId, BaixarBemRequest request, CancellationToken ct = default)
    {
        var patrimonio = await _patrimonioRepository.GetByIdAsync(patrimonioId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Patrimonio), patrimonioId);

        if (patrimonio.Baixado == 1)
            throw new BusinessRuleException("Este bem já está baixado.");
        if (request.Valor < 0)
            throw new BusinessRuleException("Valor da baixa não pode ser negativo.");

        var baixa = new BaixaBem
        {
            Data = request.Data,
            Valor = request.Valor,
            Observacao = request.Observacao,
            Motivobaixaid = request.MotivoBaixaId,
            Patrimonioid = patrimonio.Id,
            Cliforid = patrimonio.Cliforid,
        };

        patrimonio.Baixado = 1;

        await _baixaBemRepository.AddAsync(baixa, ct);
        await _patrimonioRepository.UpdateAsync(patrimonio, ct);

        _logger.LogInformation("Bem {PatrimonioId} baixado (motivo {MotivoBaixaId})", patrimonioId, request.MotivoBaixaId);

        return baixa;
    }
}

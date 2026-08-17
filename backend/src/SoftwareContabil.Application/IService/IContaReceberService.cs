using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record RegistrarRecebimentoRequest(
    double ValorRecebido,
    DateTime DataRecebimento,
    string? Descricao,
    int EspecieId,
    int ContaCorrenteId);

public interface IContaReceberService
{
    Task<IReadOnlyList<ContaReceber>> GetAllAsync(CancellationToken ct = default);
    Task<ContaReceber> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaReceber> CreateAsync(ContaReceber entity, CancellationToken ct = default);
    Task UpdateAsync(int id, ContaReceber entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ContaReceber>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default);
    Task<Recebimento> ReceberAsync(int contaReceberId, RegistrarRecebimentoRequest request, CancellationToken ct = default);
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record RegistrarPagamentoRequest(
    double ValorPago,
    DateTime DataPagamento,
    string? Descricao,
    int EspecieId,
    int ContaCorrenteId);

/// <summary>
/// Regras de negócio do módulo de contas a pagar: CRUD explícito (sem base
/// genérica) + a ação de registrar pagamento, que envolve validação e duas
/// tabelas (ContaPagar e Pagamento).
/// </summary>
public interface IContaPagarService
{
    Task<IReadOnlyList<ContaPagar>> GetAllAsync(CancellationToken ct = default);
    Task<ContaPagar> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaPagar> CreateAsync(ContaPagar entity, CancellationToken ct = default);
    Task UpdateAsync(int id, ContaPagar entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ContaPagar>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default);
    Task<Pagamento> PagarAsync(int contaPagarId, RegistrarPagamentoRequest request, CancellationToken ct = default);
}

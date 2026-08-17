using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record MovItemRequest(int ProdutoId, int LocalId, double Quantidade, double Valor);

public record CriarMovimentacaoRequest(
    string? NotaFiscal,
    char Tipo, // 'C' = compra, 'V' = venda
    DateTime Data,
    string CliForId,
    string EmpresaId,
    IReadOnlyList<MovItemRequest> Itens);

public interface IMovimentacaoService
{
    Task<IReadOnlyList<Movimentacao>> GetAllAsync(CancellationToken ct = default);
    Task<Movimentacao> GetByIdAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<Movimentacao> CriarCompletaAsync(CriarMovimentacaoRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<MovItens>> GetItensAsync(int movimentacaoId, CancellationToken ct = default);

    /// <summary>Lista paginada — recomendado pois movimentações de compra/venda crescem indefinidamente.</summary>
    Task<IReadOnlyList<Movimentacao>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

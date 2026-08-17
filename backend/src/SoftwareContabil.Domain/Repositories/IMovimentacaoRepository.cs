using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IMovimentacaoRepository
{
    Task<IReadOnlyList<Movimentacao>> GetAllAsync(CancellationToken ct = default);
    Task<Movimentacao?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Movimentacao> AddAsync(Movimentacao entity, CancellationToken ct = default);
    Task UpdateAsync(Movimentacao entity, CancellationToken ct = default);
    Task DeleteAsync(Movimentacao entity, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    /// <summary>Persiste a movimentação, seus itens e as atualizações de saldo de
    /// estoque em uma única transação (atomicidade da operação de compra/venda).</summary>
    Task<Movimentacao> CriarComItensAsync(Movimentacao movimentacao, IReadOnlyList<MovItens> itens, CancellationToken ct = default);

    Task<IReadOnlyList<MovItens>> GetItensAsync(int movimentacaoId, CancellationToken ct = default);

    /// <summary>Lista paginada — recomendado pois movimentações de compra/venda crescem indefinidamente.</summary>
    Task<IReadOnlyList<Movimentacao>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}

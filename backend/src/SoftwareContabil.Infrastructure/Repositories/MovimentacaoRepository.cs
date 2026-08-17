using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Movimentacao> _set;

    public MovimentacaoRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Movimentacao>();
    }

    public async Task<IReadOnlyList<Movimentacao>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Movimentacao?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Movimentacao> AddAsync(Movimentacao entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Movimentacao entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Movimentacao entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<Movimentacao> CriarComItensAsync(Movimentacao movimentacao, IReadOnlyList<MovItens> itens, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        _set.Add(movimentacao);
        await _db.SaveChangesAsync(ct);

        foreach (var item in itens)
        {
            item.Movimentacaoid = movimentacao.Id;
            _db.MovItensList.Add(item);

            var saldo = await _db.Saldos.FirstOrDefaultAsync(
                s => s.Produtoid == item.Produtoid && s.Localid == item.Localid, ct);

            var fator = movimentacao.Tipo is 'C' or 'c' ? 1 : -1;

            if (saldo is null)
            {
                _db.Saldos.Add(new Saldo
                {
                    Produtoid = item.Produtoid,
                    Localid = item.Localid,
                    Qtde = fator * item.Quantidade,
                    Valor = fator * item.Quantidade * item.Valor,
                });
            }
            else
            {
                saldo.Qtde += fator * item.Quantidade;
                saldo.Valor += fator * item.Quantidade * item.Valor;
            }
        }

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return await _set.AsNoTracking().FirstAsync(m => m.Id == movimentacao.Id, ct);
    }

    public async Task<IReadOnlyList<MovItens>> GetItensAsync(int movimentacaoId, CancellationToken ct = default)
        => await _db.MovItensList.AsNoTracking()
            .Where(i => i.Movimentacaoid == movimentacaoId)
            .OrderBy(i => i.Sequencia)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Movimentacao>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .OrderByDescending(m => m.Data)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
}

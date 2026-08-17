using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class SaldoRepository : ISaldoRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Saldo> _set;

    public SaldoRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Saldo>();
    }

    public async Task<IReadOnlyList<Saldo>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Saldo?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Saldo> AddAsync(Saldo entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Saldo entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Saldo entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<Saldo?> GetByProdutoLocalAsync(int produtoId, int localId, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(s => s.Produtoid == produtoId && s.Localid == localId, ct);
}

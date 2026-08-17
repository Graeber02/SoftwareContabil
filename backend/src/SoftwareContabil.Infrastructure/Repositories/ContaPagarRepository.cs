using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class ContaPagarRepository : IContaPagarRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<ContaPagar> _set;

    public ContaPagarRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<ContaPagar>();
    }

    public async Task<IReadOnlyList<ContaPagar>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<ContaPagar?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<ContaPagar> AddAsync(ContaPagar entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(ContaPagar entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(ContaPagar entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<ContaPagar>> GetEmAbertoAsync(string cliforId, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .Where(c => c.Cliforid == cliforId && c.Saldo > 0)
            .OrderBy(c => c.Datavencimento)
            .ToListAsync(ct);
}

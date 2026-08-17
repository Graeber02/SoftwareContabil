
// Implementação EF Core do repositório de LancamentoContabil (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class LancamentoContabilRepository : ILancamentoContabilRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<LancamentoContabil> _set;

    public LancamentoContabilRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<LancamentoContabil>();
    }

    public async Task<IReadOnlyList<LancamentoContabil>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<LancamentoContabil?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<LancamentoContabil> AddAsync(LancamentoContabil entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(LancamentoContabil entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(LancamentoContabil entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<LancamentoContabil>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .OrderByDescending(l => l.Datahora)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
}

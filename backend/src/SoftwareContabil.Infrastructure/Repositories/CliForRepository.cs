
// Implementação EF Core do repositório de CliFor (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class CliForRepository : ICliForRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<CliFor> _set;

    public CliForRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<CliFor>();
    }

    public async Task<IReadOnlyList<CliFor>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<CliFor?> GetByIdAsync(string id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<CliFor> AddAsync(CliFor entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(CliFor entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(CliFor entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<CliFor>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .OrderBy(c => c.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
}

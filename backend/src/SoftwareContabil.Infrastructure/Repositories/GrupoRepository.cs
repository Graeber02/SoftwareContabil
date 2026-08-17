
// Implementação EF Core do repositório de Grupo (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class GrupoRepository : IGrupoRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Grupo> _set;

    public GrupoRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Grupo>();
    }

    public async Task<IReadOnlyList<Grupo>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Grupo?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Grupo> AddAsync(Grupo entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Grupo entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Grupo entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

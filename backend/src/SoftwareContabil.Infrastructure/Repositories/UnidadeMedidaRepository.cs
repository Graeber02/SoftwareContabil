
// Implementação EF Core do repositório de UnidadeMedida (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class UnidadeMedidaRepository : IUnidadeMedidaRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<UnidadeMedida> _set;

    public UnidadeMedidaRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<UnidadeMedida>();
    }

    public async Task<IReadOnlyList<UnidadeMedida>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<UnidadeMedida?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<UnidadeMedida> AddAsync(UnidadeMedida entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(UnidadeMedida entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(UnidadeMedida entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

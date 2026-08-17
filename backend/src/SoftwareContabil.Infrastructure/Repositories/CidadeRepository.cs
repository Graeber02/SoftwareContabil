
// Implementação EF Core do repositório de Cidade (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class CidadeRepository : ICidadeRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Cidade> _set;

    public CidadeRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Cidade>();
    }

    public async Task<IReadOnlyList<Cidade>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Cidade?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Cidade> AddAsync(Cidade entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Cidade entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Cidade entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

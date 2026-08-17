
// Implementação EF Core do repositório de Produto (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Produto> _set;

    public ProdutoRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Produto>();
    }

    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Produto> AddAsync(Produto entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Produto entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Produto entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<Produto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .OrderBy(p => p.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
}

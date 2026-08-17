
// Implementação EF Core do repositório de ContaCorrente (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<ContaCorrente> _set;

    public ContaCorrenteRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<ContaCorrente>();
    }

    public async Task<IReadOnlyList<ContaCorrente>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<ContaCorrente?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<ContaCorrente> AddAsync(ContaCorrente entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(ContaCorrente entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(ContaCorrente entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

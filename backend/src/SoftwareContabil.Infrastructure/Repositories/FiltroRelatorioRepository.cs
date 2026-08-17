
// Implementação EF Core do repositório de FiltroRelatorio (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class FiltroRelatorioRepository : IFiltroRelatorioRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<FiltroRelatorio> _set;

    public FiltroRelatorioRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<FiltroRelatorio>();
    }

    public async Task<IReadOnlyList<FiltroRelatorio>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<FiltroRelatorio?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<FiltroRelatorio> AddAsync(FiltroRelatorio entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(FiltroRelatorio entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(FiltroRelatorio entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

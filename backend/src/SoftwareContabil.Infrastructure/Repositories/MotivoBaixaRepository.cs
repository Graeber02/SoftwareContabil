
// Implementação EF Core do repositório de MotivoBaixa (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class MotivoBaixaRepository : IMotivoBaixaRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<MotivoBaixa> _set;

    public MotivoBaixaRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<MotivoBaixa>();
    }

    public async Task<IReadOnlyList<MotivoBaixa>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<MotivoBaixa?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<MotivoBaixa> AddAsync(MotivoBaixa entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(MotivoBaixa entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(MotivoBaixa entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

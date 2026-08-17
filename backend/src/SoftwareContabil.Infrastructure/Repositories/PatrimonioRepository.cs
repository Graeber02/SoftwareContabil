using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class PatrimonioRepository : IPatrimonioRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Patrimonio> _set;

    public PatrimonioRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Patrimonio>();
    }

    public async Task<IReadOnlyList<Patrimonio>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Patrimonio?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Patrimonio> AddAsync(Patrimonio entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Patrimonio entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Patrimonio entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<Patrimonio>> GetAtivosAsync(string cliforId, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .Where(p => p.Cliforid == cliforId && p.Baixado == 0)
            .ToListAsync(ct);
}

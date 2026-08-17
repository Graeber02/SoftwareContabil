
// Implementação EF Core do repositório de EstadoConservacao (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class EstadoConservacaoRepository : IEstadoConservacaoRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<EstadoConservacao> _set;

    public EstadoConservacaoRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<EstadoConservacao>();
    }

    public async Task<IReadOnlyList<EstadoConservacao>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<EstadoConservacao?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<EstadoConservacao> AddAsync(EstadoConservacao entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(EstadoConservacao entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(EstadoConservacao entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}


// Implementação EF Core do repositório de SolicitacaoRelatorio (classe própria, sem base genérica).

using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class SolicitacaoRelatorioRepository : ISolicitacaoRelatorioRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<SolicitacaoRelatorio> _set;

    public SolicitacaoRelatorioRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<SolicitacaoRelatorio>();
    }

    public async Task<IReadOnlyList<SolicitacaoRelatorio>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<SolicitacaoRelatorio?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<SolicitacaoRelatorio> AddAsync(SolicitacaoRelatorio entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(SolicitacaoRelatorio entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(SolicitacaoRelatorio entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;
}

using Dapper;
using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class ContaRepository : IContaRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Conta> _set;
    private readonly DapperContext _dapper;

    public ContaRepository(AppDbContext db, DapperContext dapper)
    {
        _db = db;
        _set = db.Set<Conta>();
        _dapper = dapper;
    }

    public async Task<IReadOnlyList<Conta>> GetAllAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task<Conta?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task<Conta> AddAsync(Conta entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Conta entity, CancellationToken ct = default)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Conta entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct) is not null;

    public async Task<IReadOnlyList<Conta>> GetByClienteAsync(string cliforId, CancellationToken ct = default)
        => await _set.AsNoTracking()
            .Where(c => c.Cliforid == cliforId)
            .OrderBy(c => c.Ordem)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<int>> GetContasFilhasIdsAsync(string cliforId, string descricao, CancellationToken ct = default)
    {
        using var conn = _dapper.CreateConnection();
        var ids = await conn.QueryAsync<int>(
            "select id from contas_filhas(@cliforId, @descricao)",
            new { cliforId, descricao });
        return ids.ToList();
    }
}

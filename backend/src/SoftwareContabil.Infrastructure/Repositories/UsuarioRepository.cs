using Microsoft.EntityFrameworkCore;
using SoftwareContabil.Domain.Entities;
using SoftwareContabil.Domain.Repositories;
using SoftwareContabil.Infrastructure.Persistence;

namespace SoftwareContabil.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;
    private readonly DbSet<Usuario> _set;

    public UsuarioRepository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<Usuario>();
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<Usuario> AddAsync(Usuario entity, CancellationToken ct = default)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }
}

using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Usuario> AddAsync(Usuario entity, CancellationToken ct = default);
}

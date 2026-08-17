using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IMovItensService
    {
        Task<IReadOnlyList<MovItens>> GetAllAsync(CancellationToken ct = default);
        Task<MovItens> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MovItens> CreateAsync(MovItens entity, CancellationToken ct = default);
        Task UpdateAsync(int id, MovItens entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

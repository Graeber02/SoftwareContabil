using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IGrupoService
    {
        Task<IReadOnlyList<Grupo>> GetAllAsync(CancellationToken ct = default);
        Task<Grupo> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Grupo> CreateAsync(Grupo entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Grupo entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

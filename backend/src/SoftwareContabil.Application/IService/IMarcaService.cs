using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IMarcaService
    {
        Task<IReadOnlyList<Marca>> GetAllAsync(CancellationToken ct = default);
        Task<Marca> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Marca> CreateAsync(Marca entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Marca entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

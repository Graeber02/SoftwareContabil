using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IEstadoService
    {
        Task<IReadOnlyList<Estado>> GetAllAsync(CancellationToken ct = default);
        Task<Estado> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Estado> CreateAsync(Estado entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Estado entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

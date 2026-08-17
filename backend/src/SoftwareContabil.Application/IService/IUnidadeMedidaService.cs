using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IUnidadeMedidaService
    {
        Task<IReadOnlyList<UnidadeMedida>> GetAllAsync(CancellationToken ct = default);
        Task<UnidadeMedida> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UnidadeMedida> CreateAsync(UnidadeMedida entity, CancellationToken ct = default);
        Task UpdateAsync(int id, UnidadeMedida entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

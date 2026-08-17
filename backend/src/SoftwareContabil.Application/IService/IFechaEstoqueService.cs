using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IFechaEstoqueService
    {
        Task<IReadOnlyList<FechaEstoque>> GetAllAsync(CancellationToken ct = default);
        Task<FechaEstoque> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FechaEstoque> CreateAsync(FechaEstoque entity, CancellationToken ct = default);
        Task UpdateAsync(int id, FechaEstoque entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IFiltroRelatorioService
    {
        Task<IReadOnlyList<FiltroRelatorio>> GetAllAsync(CancellationToken ct = default);
        Task<FiltroRelatorio> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FiltroRelatorio> CreateAsync(FiltroRelatorio entity, CancellationToken ct = default);
        Task UpdateAsync(int id, FiltroRelatorio entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

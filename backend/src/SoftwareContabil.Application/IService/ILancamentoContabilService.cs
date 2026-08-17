using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface ILancamentoContabilService
    {
        Task<IReadOnlyList<LancamentoContabil>> GetAllAsync(CancellationToken ct = default);
        Task<LancamentoContabil> GetByIdAsync(int id, CancellationToken ct = default);
        Task<LancamentoContabil> CreateAsync(LancamentoContabil entity, CancellationToken ct = default);
        Task UpdateAsync(int id, LancamentoContabil entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        /// <summary>Lista paginada — recomendado pois lançamentos contábeis crescem indefinidamente.</summary>
        Task<IReadOnlyList<LancamentoContabil>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    }
}

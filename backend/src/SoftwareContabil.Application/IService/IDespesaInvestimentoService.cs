using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IDespesaInvestimentoService
    {
        Task<IReadOnlyList<DespesaInvestimento>> GetAllAsync(CancellationToken ct = default);
        Task<DespesaInvestimento> GetByIdAsync(int id, CancellationToken ct = default);
        Task<DespesaInvestimento> CreateAsync(DespesaInvestimento entity, CancellationToken ct = default);
        Task UpdateAsync(int id, DespesaInvestimento entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

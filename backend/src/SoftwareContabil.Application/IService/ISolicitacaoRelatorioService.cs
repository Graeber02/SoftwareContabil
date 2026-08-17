using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface ISolicitacaoRelatorioService
    {
        Task<IReadOnlyList<SolicitacaoRelatorio>> GetAllAsync(CancellationToken ct = default);
        Task<SolicitacaoRelatorio> GetByIdAsync(int id, CancellationToken ct = default);
        Task<SolicitacaoRelatorio> CreateAsync(SolicitacaoRelatorio entity, CancellationToken ct = default);
        Task UpdateAsync(int id, SolicitacaoRelatorio entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

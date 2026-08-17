using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IHistoricoService
    {
        Task<IReadOnlyList<Historico>> GetAllAsync(CancellationToken ct = default);
        Task<Historico> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Historico> CreateAsync(Historico entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Historico entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

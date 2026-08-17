using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IRelatorioService
    {
        Task<IReadOnlyList<Relatorio>> GetAllAsync(CancellationToken ct = default);
        Task<Relatorio> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Relatorio> CreateAsync(Relatorio entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Relatorio entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

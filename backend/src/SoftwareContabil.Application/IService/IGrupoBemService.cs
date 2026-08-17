using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IGrupoBemService
    {
        Task<IReadOnlyList<GrupoBem>> GetAllAsync(CancellationToken ct = default);
        Task<GrupoBem> GetByIdAsync(int id, CancellationToken ct = default);
        Task<GrupoBem> CreateAsync(GrupoBem entity, CancellationToken ct = default);
        Task UpdateAsync(int id, GrupoBem entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

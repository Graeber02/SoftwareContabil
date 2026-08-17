using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IDepreciacaoService
    {
        Task<IReadOnlyList<Depreciacao>> GetAllAsync(CancellationToken ct = default);
        Task<Depreciacao> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Depreciacao> CreateAsync(Depreciacao entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Depreciacao entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

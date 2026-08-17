using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IEspecieService
    {
        Task<IReadOnlyList<Especie>> GetAllAsync(CancellationToken ct = default);
        Task<Especie> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Especie> CreateAsync(Especie entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Especie entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

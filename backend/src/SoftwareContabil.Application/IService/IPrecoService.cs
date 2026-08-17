using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IPrecoService
    {
        Task<IReadOnlyList<Preco>> GetAllAsync(CancellationToken ct = default);
        Task<Preco> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Preco> CreateAsync(Preco entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Preco entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

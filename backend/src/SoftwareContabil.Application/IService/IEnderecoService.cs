using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IEnderecoService
    {
        Task<IReadOnlyList<Endereco>> GetAllAsync(CancellationToken ct = default);
        Task<Endereco> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Endereco> CreateAsync(Endereco entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Endereco entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IRecebimentoService
    {
        Task<IReadOnlyList<Recebimento>> GetAllAsync(CancellationToken ct = default);
        Task<Recebimento> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Recebimento> CreateAsync(Recebimento entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Recebimento entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

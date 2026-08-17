using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IPagamentoService
    {
        Task<IReadOnlyList<Pagamento>> GetAllAsync(CancellationToken ct = default);
        Task<Pagamento> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Pagamento> CreateAsync(Pagamento entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Pagamento entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

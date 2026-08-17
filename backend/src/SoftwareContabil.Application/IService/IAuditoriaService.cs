using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IAuditoriaService
    {
        Task<IReadOnlyList<Auditoria>> GetAllAsync(CancellationToken ct = default);
        Task<Auditoria> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Auditoria> CreateAsync(Auditoria entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Auditoria entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

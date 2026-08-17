using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface ILocalService
    {
        Task<IReadOnlyList<Local>> GetAllAsync(CancellationToken ct = default);
        Task<Local> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Local> CreateAsync(Local entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Local entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

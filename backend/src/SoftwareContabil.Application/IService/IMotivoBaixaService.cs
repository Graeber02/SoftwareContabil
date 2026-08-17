using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IMotivoBaixaService
    {
        Task<IReadOnlyList<MotivoBaixa>> GetAllAsync(CancellationToken ct = default);
        Task<MotivoBaixa> GetByIdAsync(int id, CancellationToken ct = default);
        Task<MotivoBaixa> CreateAsync(MotivoBaixa entity, CancellationToken ct = default);
        Task UpdateAsync(int id, MotivoBaixa entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

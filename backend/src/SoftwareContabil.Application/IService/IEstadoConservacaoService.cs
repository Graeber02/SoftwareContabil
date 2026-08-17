using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IEstadoConservacaoService
    {
        Task<IReadOnlyList<EstadoConservacao>> GetAllAsync(CancellationToken ct = default);
        Task<EstadoConservacao> GetByIdAsync(int id, CancellationToken ct = default);
        Task<EstadoConservacao> CreateAsync(EstadoConservacao entity, CancellationToken ct = default);
        Task UpdateAsync(int id, EstadoConservacao entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}

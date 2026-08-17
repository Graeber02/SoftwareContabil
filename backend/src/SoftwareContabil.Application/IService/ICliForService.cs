using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface ICliForService
    {
        Task<IReadOnlyList<CliFor>> GetAllAsync(CancellationToken ct = default);
        Task<CliFor> GetByIdAsync(string id, CancellationToken ct = default);
        Task<CliFor> CreateAsync(CliFor entity, CancellationToken ct = default);
        Task UpdateAsync(string id, CliFor entity, CancellationToken ct = default);
        Task DeleteAsync(string id, CancellationToken ct = default);

        /// <summary>Lista paginada — recomendado para cadastros com muitos registros (clientes/fornecedores).</summary>
        Task<IReadOnlyList<CliFor>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    }
}

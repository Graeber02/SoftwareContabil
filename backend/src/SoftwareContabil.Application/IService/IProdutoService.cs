using SoftwareContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareContabil.Application.IService
{
    public interface IProdutoService
    {
        Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct = default);
        Task<Produto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Produto> CreateAsync(Produto entity, CancellationToken ct = default);
        Task UpdateAsync(int id, Produto entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        /// <summary>Lista paginada — recomendado quando o catálogo de produtos crescer muito.</summary>
        Task<IReadOnlyList<Produto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    }
}

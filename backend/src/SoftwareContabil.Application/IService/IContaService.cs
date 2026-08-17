using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Application.IService;

public record ContaTreeNode(int Id, string Descricao, bool Sintetica, int Ordem, int? ContaPaiId)
{
    public List<ContaTreeNode> Filhos { get; } = new();
}

public interface IContaService
{
    Task<IReadOnlyList<Conta>> GetAllAsync(CancellationToken ct = default);
    Task<Conta> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Conta> CreateAsync(Conta entity, CancellationToken ct = default);
    Task UpdateAsync(int id, Conta entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ContaTreeNode>> GetArvoreAsync(string cliforId, CancellationToken ct = default);
    Task<IReadOnlyList<int>> GetContasFilhasAsync(string cliforId, string descricao, CancellationToken ct = default);
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

public record MovItemDto(int ProdutoId, int LocalId, double Quantidade, double Valor);

public record CriarMovimentacaoDto(
    string? NotaFiscal, char Tipo, DateTime Data, string CliForId, string EmpresaId, List<MovItemDto> Itens);

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public MovimentacaoController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movimentacao>>> GetAll(CancellationToken ct)
        => Ok(await _movimentacaoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Movimentacao>> GetById(int id, CancellationToken ct)
        => Ok(await _movimentacaoService.GetByIdAsync(id, ct));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _movimentacaoService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpPost("completa")]
    public async Task<ActionResult<Movimentacao>> CriarCompleta([FromBody] CriarMovimentacaoDto dto, CancellationToken ct)
    {
        var request = new CriarMovimentacaoRequest(
            dto.NotaFiscal, dto.Tipo, dto.Data, dto.CliForId, dto.EmpresaId,
            dto.Itens.Select(i => new MovItemRequest(i.ProdutoId, i.LocalId, i.Quantidade, i.Valor)).ToList());
        return Ok(await _movimentacaoService.CriarCompletaAsync(request, ct));
    }

    [HttpGet("{id}/itens")]
    public async Task<ActionResult<IEnumerable<MovItens>>> Itens(int id, CancellationToken ct)
        => Ok(await _movimentacaoService.GetItensAsync(id, ct));

    /// <summary>Lista paginada — use para não carregar o histórico inteiro de movimentações de uma vez.</summary>
    [HttpGet("paginado")]
    public async Task<ActionResult<IEnumerable<Movimentacao>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
        => Ok(await _movimentacaoService.GetPagedAsync(page, pageSize, ct));
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

public record RegistrarPagamentoDto(double ValorPago, DateTime DataPagamento, string? Descricao, int EspecieId, int ContaCorrenteId);

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContaPagarController : ControllerBase
{
    private readonly IContaPagarService _contaPagarService;

    public ContaPagarController(IContaPagarService contaPagarService)
    {
        _contaPagarService = contaPagarService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContaPagar>>> GetAll(CancellationToken ct)
        => Ok(await _contaPagarService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaPagar>> GetById(int id, CancellationToken ct)
        => Ok(await _contaPagarService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<ContaPagar>> Create([FromBody] ContaPagar entity, CancellationToken ct)
    {
        var created = await _contaPagarService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContaPagar entity, CancellationToken ct)
    {
        await _contaPagarService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _contaPagarService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpGet("em-aberto/{cliforId}")]
    public async Task<ActionResult<IEnumerable<ContaPagar>>> EmAberto(string cliforId, CancellationToken ct)
        => Ok(await _contaPagarService.GetEmAbertoAsync(cliforId, ct));

    [HttpPost("{id}/pagar")]
    public async Task<ActionResult<Pagamento>> Pagar(int id, [FromBody] RegistrarPagamentoDto dto, CancellationToken ct)
    {
        var request = new RegistrarPagamentoRequest(dto.ValorPago, dto.DataPagamento, dto.Descricao, dto.EspecieId, dto.ContaCorrenteId);
        return Ok(await _contaPagarService.PagarAsync(id, request, ct));
    }
}

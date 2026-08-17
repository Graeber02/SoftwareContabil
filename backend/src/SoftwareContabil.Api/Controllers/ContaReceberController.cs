using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

public record RegistrarRecebimentoDto(double ValorRecebido, DateTime DataRecebimento, string? Descricao, int EspecieId, int ContaCorrenteId);

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContaReceberController : ControllerBase
{
    private readonly IContaReceberService _contaReceberService;

    public ContaReceberController(IContaReceberService contaReceberService)
    {
        _contaReceberService = contaReceberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContaReceber>>> GetAll(CancellationToken ct)
        => Ok(await _contaReceberService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaReceber>> GetById(int id, CancellationToken ct)
        => Ok(await _contaReceberService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<ContaReceber>> Create([FromBody] ContaReceber entity, CancellationToken ct)
    {
        var created = await _contaReceberService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContaReceber entity, CancellationToken ct)
    {
        await _contaReceberService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _contaReceberService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpGet("em-aberto/{cliforId}")]
    public async Task<ActionResult<IEnumerable<ContaReceber>>> EmAberto(string cliforId, CancellationToken ct)
        => Ok(await _contaReceberService.GetEmAbertoAsync(cliforId, ct));

    [HttpPost("{id}/receber")]
    public async Task<ActionResult<Recebimento>> Receber(int id, [FromBody] RegistrarRecebimentoDto dto, CancellationToken ct)
    {
        var request = new RegistrarRecebimentoRequest(dto.ValorRecebido, dto.DataRecebimento, dto.Descricao, dto.EspecieId, dto.ContaCorrenteId);
        return Ok(await _contaReceberService.ReceberAsync(id, request, ct));
    }
}

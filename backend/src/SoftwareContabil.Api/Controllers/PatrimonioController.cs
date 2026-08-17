using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

public record BaixarBemDto(DateTime Data, double Valor, string? Observacao, int MotivoBaixaId);

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatrimonioController : ControllerBase
{
    private readonly IPatrimonioService _patrimonioService;

    public PatrimonioController(IPatrimonioService patrimonioService)
    {
        _patrimonioService = patrimonioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patrimonio>>> GetAll(CancellationToken ct)
        => Ok(await _patrimonioService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Patrimonio>> GetById(int id, CancellationToken ct)
        => Ok(await _patrimonioService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Patrimonio>> Create([FromBody] Patrimonio entity, CancellationToken ct)
    {
        var created = await _patrimonioService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Patrimonio entity, CancellationToken ct)
    {
        await _patrimonioService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _patrimonioService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpGet("ativos/{cliforId}")]
    public async Task<ActionResult<IEnumerable<Patrimonio>>> Ativos(string cliforId, CancellationToken ct)
        => Ok(await _patrimonioService.GetAtivosAsync(cliforId, ct));

    [HttpPost("{id}/baixar")]
    public async Task<ActionResult<BaixaBem>> Baixar(int id, [FromBody] BaixarBemDto dto, CancellationToken ct)
    {
        var request = new BaixarBemRequest(dto.Data, dto.Valor, dto.Observacao, dto.MotivoBaixaId);
        return Ok(await _patrimonioService.BaixarAsync(id, request, ct));
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DespesaInvestimentoController : ControllerBase
{
    private readonly IDespesaInvestimentoService _despesaInvestimentoService;

    public DespesaInvestimentoController(IDespesaInvestimentoService despesaInvestimentoService)
    {
        _despesaInvestimentoService = despesaInvestimentoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DespesaInvestimento>>> GetAll(CancellationToken ct)
        => Ok(await _despesaInvestimentoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<DespesaInvestimento>> GetById(int id, CancellationToken ct)
        => Ok(await _despesaInvestimentoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<DespesaInvestimento>> Create([FromBody] DespesaInvestimento entity, CancellationToken ct)
    {
        var created = await _despesaInvestimentoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DespesaInvestimento entity, CancellationToken ct)
    {
        await _despesaInvestimentoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _despesaInvestimentoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

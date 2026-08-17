using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HistoricoController : ControllerBase
{
    private readonly IHistoricoService _historicoService;

    public HistoricoController(IHistoricoService historicoService)
    {
        _historicoService = historicoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Historico>>> GetAll(CancellationToken ct)
        => Ok(await _historicoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Historico>> GetById(int id, CancellationToken ct)
        => Ok(await _historicoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Historico>> Create([FromBody] Historico entity, CancellationToken ct)
    {
        var created = await _historicoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Historico entity, CancellationToken ct)
    {
        await _historicoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _historicoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

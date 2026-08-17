
// Controller REST de Depreciacao: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepreciacaoController : ControllerBase
{
    private readonly IDepreciacaoService _depreciacaoService;

    public DepreciacaoController(IDepreciacaoService depreciacaoService)
    {
        _depreciacaoService = depreciacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Depreciacao>>> GetAll(CancellationToken ct)
        => Ok(await _depreciacaoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Depreciacao>> GetById(int id, CancellationToken ct)
        => Ok(await _depreciacaoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Depreciacao>> Create([FromBody] Depreciacao entity, CancellationToken ct)
    {
        var created = await _depreciacaoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Depreciacao entity, CancellationToken ct)
    {
        await _depreciacaoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _depreciacaoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

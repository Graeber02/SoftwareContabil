
// Controller REST de Especie: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EspecieController : ControllerBase
{
    private readonly IEspecieService _especieService;

    public EspecieController(IEspecieService especieService)
    {
        _especieService = especieService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Especie>>> GetAll(CancellationToken ct)
        => Ok(await _especieService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Especie>> GetById(int id, CancellationToken ct)
        => Ok(await _especieService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Especie>> Create([FromBody] Especie entity, CancellationToken ct)
    {
        var created = await _especieService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Especie entity, CancellationToken ct)
    {
        await _especieService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _especieService.DeleteAsync(id, ct);
        return NoContent();
    }
}

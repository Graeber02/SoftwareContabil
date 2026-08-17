
// Controller REST de Grupo: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _service;

    public GrupoController(IGrupoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Grupo>> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Grupo>> Create([FromBody] Grupo entity, CancellationToken ct)
    {
        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Grupo entity, CancellationToken ct)
    {
        await _service.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}

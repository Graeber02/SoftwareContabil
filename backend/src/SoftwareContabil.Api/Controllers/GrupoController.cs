using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _grupoService;

    public GrupoController(IGrupoService grupoService)
    {
        _grupoService = grupoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grupo>>> GetAll(CancellationToken ct)
        => Ok(await _grupoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Grupo>> GetById(int id, CancellationToken ct)
        => Ok(await _grupoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Grupo>> Create([FromBody] Grupo entity, CancellationToken ct)
    {
        var created = await _grupoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Grupo entity, CancellationToken ct)
    {
        await _grupoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _grupoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

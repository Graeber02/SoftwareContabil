using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstadoController : ControllerBase
{
    private readonly IEstadoService _estadoService;

    public EstadoController(IEstadoService estadoService)
    {
        _estadoService = estadoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estado>>> GetAll(CancellationToken ct)
        => Ok(await _estadoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Estado>> GetById(int id, CancellationToken ct)
        => Ok(await _estadoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Estado>> Create([FromBody] Estado entity, CancellationToken ct)
    {
        var created = await _estadoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Estado entity, CancellationToken ct)
    {
        await _estadoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _estadoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

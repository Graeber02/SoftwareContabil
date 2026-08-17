using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MarcaController : ControllerBase
{
    private readonly IMarcaService _marcaService;

    public MarcaController(IMarcaService marcaService)
    {
        _marcaService = marcaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marca>>> GetAll(CancellationToken ct)
        => Ok(await _marcaService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Marca>> GetById(int id, CancellationToken ct)
        => Ok(await _marcaService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Marca>> Create([FromBody] Marca entity, CancellationToken ct)
    {
        var created = await _marcaService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Marca entity, CancellationToken ct)
    {
        await _marcaService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _marcaService.DeleteAsync(id, ct);
        return NoContent();
    }
}

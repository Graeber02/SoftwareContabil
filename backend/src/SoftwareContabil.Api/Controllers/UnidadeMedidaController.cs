using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UnidadeMedidaController : ControllerBase
{
    private readonly IUnidadeMedidaService _unidademedidaService;

    public UnidadeMedidaController(IUnidadeMedidaService unidademedidaService)
    {
        _unidademedidaService = unidademedidaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadeMedida>>> GetAll(CancellationToken ct)
        => Ok(await _unidademedidaService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<UnidadeMedida>> GetById(int id, CancellationToken ct)
        => Ok(await _unidademedidaService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<UnidadeMedida>> Create([FromBody] UnidadeMedida entity, CancellationToken ct)
    {
        var created = await _unidademedidaService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UnidadeMedida entity, CancellationToken ct)
    {
        await _unidademedidaService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _unidademedidaService.DeleteAsync(id, ct);
        return NoContent();
    }
}

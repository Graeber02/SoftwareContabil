using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstadoConservacaoController : ControllerBase
{
    private readonly IEstadoConservacaoService _estadoConservacaoService;

    public EstadoConservacaoController(IEstadoConservacaoService estadoConservacaoService)
    {
        _estadoConservacaoService = estadoConservacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoConservacao>>> GetAll(CancellationToken ct)
        => Ok(await _estadoConservacaoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<EstadoConservacao>> GetById(int id, CancellationToken ct)
        => Ok(await _estadoConservacaoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<EstadoConservacao>> Create([FromBody] EstadoConservacao entity, CancellationToken ct)
    {
        var created = await _estadoConservacaoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EstadoConservacao entity, CancellationToken ct)
    {
        await _estadoConservacaoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _estadoConservacaoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

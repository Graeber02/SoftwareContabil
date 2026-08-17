
// Controller REST de Cidade: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CidadeController : ControllerBase
{
    private readonly ICidadeService _cidadeService;

    public CidadeController(ICidadeService cidadeService)
    {
        _cidadeService = cidadeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cidade>>> GetAll(CancellationToken ct)
        => Ok(await _cidadeService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Cidade>> GetById(int id, CancellationToken ct)
        => Ok(await _cidadeService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Cidade>> Create([FromBody] Cidade entity, CancellationToken ct)
    {
        var created = await _cidadeService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Cidade entity, CancellationToken ct)
    {
        await _cidadeService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _cidadeService.DeleteAsync(id, ct);
        return NoContent();
    }
}

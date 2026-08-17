
// Controller REST de Auditoria: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auditoria>>> GetAll(CancellationToken ct)
        => Ok(await _auditoriaService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Auditoria>> GetById(int id, CancellationToken ct)
        => Ok(await _auditoriaService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Auditoria>> Create([FromBody] Auditoria entity, CancellationToken ct)
    {
        var created = await _auditoriaService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Auditoria entity, CancellationToken ct)
    {
        await _auditoriaService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _auditoriaService.DeleteAsync(id, ct);
        return NoContent();
    }
}

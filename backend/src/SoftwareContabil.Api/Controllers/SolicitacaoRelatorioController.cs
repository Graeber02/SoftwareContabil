
// Controller REST de SolicitacaoRelatorio: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SolicitacaoRelatorioController : ControllerBase
{
    private readonly ISolicitacaoRelatorioService _service;

    public SolicitacaoRelatorioController(ISolicitacaoRelatorioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitacaoRelatorio>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitacaoRelatorio>> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<SolicitacaoRelatorio>> Create([FromBody] SolicitacaoRelatorio entity, CancellationToken ct)
    {
        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SolicitacaoRelatorio entity, CancellationToken ct)
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SolicitacaoRelatorioController : ControllerBase
{
    private readonly ISolicitacaoRelatorioService _solicitacaorelatorioService;

    public SolicitacaoRelatorioController(ISolicitacaoRelatorioService solicitacaorelatorioService)
    {
        _solicitacaorelatorioService = solicitacaorelatorioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitacaoRelatorio>>> GetAll(CancellationToken ct)
        => Ok(await _solicitacaorelatorioService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitacaoRelatorio>> GetById(int id, CancellationToken ct)
        => Ok(await _solicitacaorelatorioService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<SolicitacaoRelatorio>> Create([FromBody] SolicitacaoRelatorio entity, CancellationToken ct)
    {
        var created = await _solicitacaorelatorioService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SolicitacaoRelatorio entity, CancellationToken ct)
    {
        await _solicitacaorelatorioService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _solicitacaorelatorioService.DeleteAsync(id, ct);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LancamentoContabilController : ControllerBase
{
    private readonly ILancamentoContabilService _lancamentocontabilService;

    public LancamentoContabilController(ILancamentoContabilService lancamentocontabilService)
    {
        _lancamentocontabilService = lancamentocontabilService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LancamentoContabil>>> GetAll(CancellationToken ct)
        => Ok(await _lancamentocontabilService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<LancamentoContabil>> GetById(int id, CancellationToken ct)
        => Ok(await _lancamentocontabilService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<LancamentoContabil>> Create([FromBody] LancamentoContabil entity, CancellationToken ct)
    {
        var created = await _lancamentocontabilService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LancamentoContabil entity, CancellationToken ct)
    {
        await _lancamentocontabilService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _lancamentocontabilService.DeleteAsync(id, ct);
        return NoContent();
    }
}

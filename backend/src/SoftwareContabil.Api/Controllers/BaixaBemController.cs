
// Controller REST de BaixaBem: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BaixaBemController : ControllerBase
{
    private readonly IBaixaBemService _baixaBemService;

    public BaixaBemController(IBaixaBemService baixaBemService)
    {
        _baixaBemService = baixaBemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BaixaBem>>> GetAll(CancellationToken ct)
        => Ok(await _baixaBemService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<BaixaBem>> GetById(int id, CancellationToken ct)
        => Ok(await _baixaBemService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<BaixaBem>> Create([FromBody] BaixaBem entity, CancellationToken ct)
    {
        var created = await _baixaBemService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BaixaBem entity, CancellationToken ct)
    {
        await _baixaBemService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _baixaBemService.DeleteAsync(id, ct);
        return NoContent();
    }
}

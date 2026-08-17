using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PrecoController : ControllerBase
{
    private readonly IPrecoService _precoService;

    public PrecoController(IPrecoService precoService)
    {
        _precoService = precoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Preco>>> GetAll(CancellationToken ct)
        => Ok(await _precoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Preco>> GetById(int id, CancellationToken ct)
        => Ok(await _precoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Preco>> Create([FromBody] Preco entity, CancellationToken ct)
    {
        var created = await _precoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Preco entity, CancellationToken ct)
    {
        await _precoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _precoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

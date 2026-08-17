using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecebimentoController : ControllerBase
{
    private readonly IRecebimentoService _recebimentoService;

    public RecebimentoController(IRecebimentoService recebimentoService)
    {
        _recebimentoService = recebimentoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recebimento>>> GetAll(CancellationToken ct)
        => Ok(await _recebimentoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Recebimento>> GetById(int id, CancellationToken ct)
        => Ok(await _recebimentoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Recebimento>> Create([FromBody] Recebimento entity, CancellationToken ct)
    {
        var created = await _recebimentoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Recebimento entity, CancellationToken ct)
    {
        await _recebimentoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _recebimentoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

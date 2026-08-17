using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SaldoController : ControllerBase
{
    private readonly ISaldoService _saldoService;

    public SaldoController(ISaldoService saldoService)
    {
        _saldoService = saldoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Saldo>>> GetAll(CancellationToken ct)
        => Ok(await _saldoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Saldo>> GetById(int id, CancellationToken ct)
        => Ok(await _saldoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Saldo>> Create([FromBody] Saldo entity, CancellationToken ct)
    {
        var created = await _saldoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Saldo entity, CancellationToken ct)
    {
        await _saldoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _saldoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

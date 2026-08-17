using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MotivoBaixaController : ControllerBase
{
    private readonly IMotivoBaixaService _motivobaixaService;

    public MotivoBaixaController(IMotivoBaixaService motivobaixaService)
    {
        _motivobaixaService = motivobaixaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotivoBaixa>>> GetAll(CancellationToken ct)
        => Ok(await _motivobaixaService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<MotivoBaixa>> GetById(int id, CancellationToken ct)
        => Ok(await _motivobaixaService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<MotivoBaixa>> Create([FromBody] MotivoBaixa entity, CancellationToken ct)
    {
        var created = await _motivobaixaService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MotivoBaixa entity, CancellationToken ct)
    {
        await _motivobaixaService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _motivobaixaService.DeleteAsync(id, ct);
        return NoContent();
    }
}

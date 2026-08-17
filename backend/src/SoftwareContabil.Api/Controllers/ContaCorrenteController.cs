
// Controller REST de ContaCorrente: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IContaCorrenteService _contaCorrenteService;

    public ContaCorrenteController(IContaCorrenteService contaCorrenteService)
    {
        _contaCorrenteService = contaCorrenteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContaCorrente>>> GetAll(CancellationToken ct)
        => Ok(await _contaCorrenteService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaCorrente>> GetById(int id, CancellationToken ct)
        => Ok(await _contaCorrenteService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<ContaCorrente>> Create([FromBody] ContaCorrente entity, CancellationToken ct)
    {
        var created = await _contaCorrenteService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContaCorrente entity, CancellationToken ct)
    {
        await _contaCorrenteService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _contaCorrenteService.DeleteAsync(id, ct);
        return NoContent();
    }
}

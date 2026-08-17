using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CentroCustoController : ControllerBase
{
    private readonly ICentroCustoService _centroCustoService;

    public CentroCustoController(ICentroCustoService centroCustoService)
    {
        _centroCustoService = centroCustoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CentroCusto>>> GetAll(CancellationToken ct)
        => Ok(await _centroCustoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<CentroCusto>> GetById(int id, CancellationToken ct)
        => Ok(await _centroCustoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<CentroCusto>> Create([FromBody] CentroCusto entity, CancellationToken ct)
    {
        var created = await _centroCustoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CentroCusto entity, CancellationToken ct)
    {
        await _centroCustoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _centroCustoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

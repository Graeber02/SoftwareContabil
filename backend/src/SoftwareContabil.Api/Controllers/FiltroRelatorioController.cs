using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FiltroRelatorioController : ControllerBase
{
    private readonly IFiltroRelatorioService _filtroRelatorioService;

    public FiltroRelatorioController(IFiltroRelatorioService filtroRelatorioService)
    {
        _filtroRelatorioService = filtroRelatorioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FiltroRelatorio>>> GetAll(CancellationToken ct)
        => Ok(await _filtroRelatorioService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<FiltroRelatorio>> GetById(int id, CancellationToken ct)
        => Ok(await _filtroRelatorioService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<FiltroRelatorio>> Create([FromBody] FiltroRelatorio entity, CancellationToken ct)
    {
        var created = await _filtroRelatorioService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FiltroRelatorio entity, CancellationToken ct)
    {
        await _filtroRelatorioService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _filtroRelatorioService.DeleteAsync(id, ct);
        return NoContent();
    }
}

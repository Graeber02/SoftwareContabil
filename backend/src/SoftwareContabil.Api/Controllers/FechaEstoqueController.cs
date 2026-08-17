
// Controller REST de FechaEstoque: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FechaEstoqueController : ControllerBase
{
    private readonly IFechaEstoqueService _fechaEstoqueService;

    public FechaEstoqueController(IFechaEstoqueService fechaEstoqueService)
    {
        _fechaEstoqueService = fechaEstoqueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FechaEstoque>>> GetAll(CancellationToken ct)
        => Ok(await _fechaEstoqueService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<FechaEstoque>> GetById(int id, CancellationToken ct)
        => Ok(await _fechaEstoqueService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<FechaEstoque>> Create([FromBody] FechaEstoque entity, CancellationToken ct)
    {
        var created = await _fechaEstoqueService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FechaEstoque entity, CancellationToken ct)
    {
        await _fechaEstoqueService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _fechaEstoqueService.DeleteAsync(id, ct);
        return NoContent();
    }
}

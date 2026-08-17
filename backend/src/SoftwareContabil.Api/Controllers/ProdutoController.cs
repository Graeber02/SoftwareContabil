
// Controller REST de Produto: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutoController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Produto>> Create([FromBody] Produto entity, CancellationToken ct)
    {
        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Produto entity, CancellationToken ct)
    {
        await _service.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }

    /// <summary>Lista paginada — use para não carregar o catálogo inteiro de uma vez quando ele crescer muito.</summary>
    [HttpGet("paginado")]
    public async Task<ActionResult<IEnumerable<Produto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, ct));
}

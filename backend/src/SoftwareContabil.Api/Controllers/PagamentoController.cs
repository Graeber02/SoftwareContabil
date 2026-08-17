using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PagamentoController : ControllerBase
{
    private readonly IPagamentoService _pagamentoService;

    public PagamentoController(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pagamento>>> GetAll(CancellationToken ct)
        => Ok(await _pagamentoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Pagamento>> GetById(int id, CancellationToken ct)
        => Ok(await _pagamentoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Pagamento>> Create([FromBody] Pagamento entity, CancellationToken ct)
    {
        var created = await _pagamentoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Pagamento entity, CancellationToken ct)
    {
        await _pagamentoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _pagamentoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

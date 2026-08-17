using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MovItensController : ControllerBase
{
    private readonly IMovItensService _movitensService;

    public MovItensController(IMovItensService movitensService)
    {
        _movitensService = movitensService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovItens>>> GetAll(CancellationToken ct)
        => Ok(await _movitensService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<MovItens>> GetById(int id, CancellationToken ct)
        => Ok(await _movitensService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<MovItens>> Create([FromBody] MovItens entity, CancellationToken ct)
    {
        var created = await _movitensService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MovItens entity, CancellationToken ct)
    {
        await _movitensService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _movitensService.DeleteAsync(id, ct);
        return NoContent();
    }
}

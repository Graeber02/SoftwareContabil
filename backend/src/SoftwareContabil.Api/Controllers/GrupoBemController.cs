using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GrupoBemController : ControllerBase
{
    private readonly IGrupoBemService _grupoBemService;

    public GrupoBemController(IGrupoBemService grupoBemService)
    {
        _grupoBemService = grupoBemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GrupoBem>>> GetAll(CancellationToken ct)
        => Ok(await _grupoBemService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<GrupoBem>> GetById(int id, CancellationToken ct)
        => Ok(await _grupoBemService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<GrupoBem>> Create([FromBody] GrupoBem entity, CancellationToken ct)
    {
        var created = await _grupoBemService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GrupoBem entity, CancellationToken ct)
    {
        await _grupoBemService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _grupoBemService.DeleteAsync(id, ct);
        return NoContent();
    }
}

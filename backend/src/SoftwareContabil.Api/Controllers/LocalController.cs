using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LocalController : ControllerBase
{
    private readonly ILocalService _localService;

    public LocalController(ILocalService localService)
    {
        _localService = localService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Local>>> GetAll(CancellationToken ct)
        => Ok(await _localService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Local>> GetById(int id, CancellationToken ct)
        => Ok(await _localService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Local>> Create([FromBody] Local entity, CancellationToken ct)
    {
        var created = await _localService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Local entity, CancellationToken ct)
    {
        await _localService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _localService.DeleteAsync(id, ct);
        return NoContent();
    }
}

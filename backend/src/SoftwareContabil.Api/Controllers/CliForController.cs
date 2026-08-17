
// Controller REST de CliFor: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CliForController : ControllerBase
{
    private readonly ICliForService _cliForService;

    public CliForController(ICliForService cliForService)
    {
        _cliForService = cliForService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CliFor>>> GetAll(CancellationToken ct)
        => Ok(await _cliForService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<CliFor>> GetById(string id, CancellationToken ct)
        => Ok(await _cliForService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<CliFor>> Create([FromBody] CliFor entity, CancellationToken ct)
    {
        var created = await _cliForService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CliFor entity, CancellationToken ct)
    {
        await _cliForService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _cliForService.DeleteAsync(id, ct);
        return NoContent();
    }

    /// <summary>Lista paginada — use para não carregar o cadastro inteiro de uma vez quando ele crescer muito.</summary>
    [HttpGet("paginado")]
    public async Task<ActionResult<IEnumerable<CliFor>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
        => Ok(await _cliForService.GetPagedAsync(page, pageSize, ct));
}


// Controller REST de Endereco: endpoints explícitos (sem controller base genérica).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Application.Service;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase
{
    private readonly IEnderecoService _enderecoService;

    public EnderecoController(IEnderecoService enderecoService)
    {
        _enderecoService = enderecoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Endereco>>> GetAll(CancellationToken ct)
        => Ok(await _enderecoService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Endereco>> GetById(int id, CancellationToken ct)
        => Ok(await _enderecoService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Endereco>> Create([FromBody] Endereco entity, CancellationToken ct)
    {
        var created = await _enderecoService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Endereco entity, CancellationToken ct)
    {
        await _enderecoService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _enderecoService.DeleteAsync(id, ct);
        return NoContent();
    }
}

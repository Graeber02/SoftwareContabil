using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;
using SoftwareContabil.Domain.Entities;

namespace SoftwareContabil.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly IContaService _contaService;

    public ContaController(IContaService contaService)
    {
        _contaService = contaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Conta>>> GetAll(CancellationToken ct)
        => Ok(await _contaService.GetAllAsync(ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<Conta>> GetById(int id, CancellationToken ct)
        => Ok(await _contaService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<Conta>> Create([FromBody] Conta entity, CancellationToken ct)
    {
        var created = await _contaService.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Conta entity, CancellationToken ct)
    {
        await _contaService.UpdateAsync(id, entity, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _contaService.DeleteAsync(id, ct);
        return NoContent();
    }

    /// <summary>Retorna o plano de contas do cliente/empresa em formato de árvore.</summary>
    [HttpGet("arvore/{cliforId}")]
    public async Task<ActionResult<IEnumerable<ContaTreeNode>>> Arvore(string cliforId, CancellationToken ct)
        => Ok(await _contaService.GetArvoreAsync(cliforId, ct));

    /// <summary>Ids das contas "filhas" de uma conta sintética, por descrição (usado pelo DRE).</summary>
    [HttpGet("filhas/{cliforId}/{descricao}")]
    public async Task<ActionResult<IEnumerable<int>>> ContasFilhas(string cliforId, string descricao, CancellationToken ct)
        => Ok(await _contaService.GetContasFilhasAsync(cliforId, descricao, ct));
}

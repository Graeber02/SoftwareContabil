using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareContabil.Application.IService;

namespace SoftwareContabil.Api.Controllers;

/// <summary>Módulo gerencial (RelatorioBean original): DRE, balancete e resumo financeiro.</summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GerencialController : ControllerBase
{
    private readonly IGerencialService _service;

    public GerencialController(IGerencialService service)
    {
        _service = service;
    }

    [HttpGet("dre/{cliforId}")]
    public async Task<ActionResult> Dre(string cliforId, CancellationToken ct)
        => Ok(await _service.GetDreAsync(cliforId, ct));

    [HttpGet("balancete/{cliforId}")]
    public async Task<ActionResult> Balancete(string cliforId, CancellationToken ct)
        => Ok(await _service.GetBalanceteAsync(cliforId, ct));

    [HttpGet("contas-pagar-receber/{cliforId}")]
    public async Task<ActionResult> ContasPagarReceber(string cliforId, CancellationToken ct)
        => Ok(await _service.GetContasPagarReceberAsync(cliforId, ct));
}

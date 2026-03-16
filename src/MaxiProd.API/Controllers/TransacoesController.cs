using MaxiProd.Application.DTOs;
using MaxiProd.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaxiProd.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var result = await _transacaoService.ObterTodosAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoRequest request, CancellationToken cancellationToken)
    {
        var result = await _transacaoService.CriarAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error.Message });

        return Created($"/api/transacoes/{result.Value.Id}", result.Value);
    }
}


using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnico.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var result = await _pessoaService.ObterTodosAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var result = await _pessoaService.ObterPorIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(new { error = result.Error.Message });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaRequest request, CancellationToken cancellationToken)
    {
        var result = await _pessoaService.CriarAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error.Message });

        return CreatedAtAction(nameof(ObterPorId), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarPessoaRequest request, CancellationToken cancellationToken)
    {
        var result = await _pessoaService.AtualizarAsync(id, request, cancellationToken);
        if (result.IsFailure)
            return result.Error.Code.Contains("NaoEncontrada")
                ? NotFound(new { error = result.Error.Message })
                : BadRequest(new { error = result.Error.Message });

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id, CancellationToken cancellationToken)
    {
        var result = await _pessoaService.DeletarAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(new { error = result.Error.Message });

        return NoContent();
    }

    [HttpGet("totais")]
    public async Task<IActionResult> ObterTotais(CancellationToken cancellationToken)
    {
        var result = await _pessoaService.ObterTotaisPorPessoaAsync(cancellationToken);
        return Ok(result.Value);
    }
}


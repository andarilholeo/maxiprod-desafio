using MaxiProd.Application.DTOs;
using MaxiProd.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaxiProd.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var result = await _categoriaService.ObterTodosAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoriaService.CriarAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error.Message });

        return Created($"/api/categorias/{result.Value.Id}", result.Value);
    }

    [HttpGet("totais")]
    public async Task<IActionResult> ObterTotais(CancellationToken cancellationToken)
    {
        var result = await _categoriaService.ObterTotaisPorCategoriaAsync(cancellationToken);
        return Ok(result.Value);
    }
}


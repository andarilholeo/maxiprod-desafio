using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Application.Interfaces;
using DesafioTecnico.Domain.Common;
using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Enums;
using DesafioTecnico.Domain.Interfaces;

namespace DesafioTecnico.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<CategoriaDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        var categorias = await _categoriaRepository.ObterTodosAsync(cancellationToken);
        var dtos = categorias.Select(c => new CategoriaDto(c.Id, c.Descricao, c.Finalidade));
        return Result.Success(dtos);
    }

    public async Task<Result<CategoriaDto>> CriarAsync(CriarCategoriaRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = ValidarCategoria(request.Descricao, request.Finalidade);
        if (validationResult.IsFailure)
            return Result.Failure<CategoriaDto>(validationResult.Error);

        var categoria = Categoria.Criar(request.Descricao, request.Finalidade);
        await _categoriaRepository.AdicionarAsync(categoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CategoriaDto(categoria.Id, categoria.Descricao, categoria.Finalidade));
    }

    public async Task<Result<TotaisCategoriasDto>> ObterTotaisPorCategoriaAsync(CancellationToken cancellationToken = default)
    {
        var categorias = await _categoriaRepository.ObterTodosAsync(cancellationToken);

        var totaisPorCategoria = categorias.Select(c => new TotalPorCategoriaDto(
            c.Id,
            c.Descricao,
            c.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor),
            c.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor),
            c.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor) -
            c.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor)
        )).ToList();

        var totalGeralReceitas = totaisPorCategoria.Sum(t => t.TotalReceitas);
        var totalGeralDespesas = totaisPorCategoria.Sum(t => t.TotalDespesas);
        var saldoLiquido = totalGeralReceitas - totalGeralDespesas;

        return Result.Success(new TotaisCategoriasDto(totaisPorCategoria, totalGeralReceitas, totalGeralDespesas, saldoLiquido));
    }

    private static Result ValidarCategoria(string descricao, Finalidade finalidade)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure(DomainErrors.Categoria.DescricaoVazia);

        if (descricao.Length > 400)
            return Result.Failure(DomainErrors.Categoria.DescricaoMuitoLonga);

        if (!Enum.IsDefined(typeof(Finalidade), finalidade))
            return Result.Failure(DomainErrors.Categoria.FinalidadeInvalida);

        return Result.Success();
    }
}


using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Application.Interfaces;
using DesafioTecnico.Domain.Common;
using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Enums;
using DesafioTecnico.Domain.Interfaces;

namespace DesafioTecnico.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<TransacaoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        var transacoes = await _transacaoRepository.ObterTodosAsync(cancellationToken);
        var dtos = transacoes.Select(t => new TransacaoDto(
            t.Id,
            t.Descricao,
            t.Valor,
            t.Tipo,
            t.CategoriaId,
            t.Categoria.Descricao,
            t.PessoaId,
            t.Pessoa.Nome
        ));
        return Result.Success(dtos);
    }

    public async Task<Result<TransacaoDto>> CriarAsync(CriarTransacaoRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Descricao))
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.DescricaoVazia);

        if (request.Descricao.Length > 400)
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.DescricaoMuitoLonga);

        if (request.Valor <= 0)
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.ValorInvalido);

        if (!Enum.IsDefined(typeof(TipoTransacao), request.Tipo))
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.TipoInvalido);

        var pessoa = await _pessoaRepository.ObterPorIdAsync(request.PessoaId, cancellationToken);
        if (pessoa is null)
            return Result.Failure<TransacaoDto>(DomainErrors.Pessoa.NaoEncontrada);

        if (pessoa.EhMenorDeIdade() && request.Tipo == TipoTransacao.Receita)
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.MenorSomenteDespesa);

        var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null)
            return Result.Failure<TransacaoDto>(DomainErrors.Categoria.NaoEncontrada);

        if (!categoria.AceitaTipoTransacao(request.Tipo))
            return Result.Failure<TransacaoDto>(DomainErrors.Transacao.CategoriaIncompativel);

        var transacao = Transacao.Criar(
            request.Descricao,
            request.Valor,
            request.Tipo,
            request.CategoriaId,
            request.PessoaId
        );

        await _transacaoRepository.AdicionarAsync(transacao, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TransacaoDto(
            transacao.Id,
            transacao.Descricao,
            transacao.Valor,
            transacao.Tipo,
            transacao.CategoriaId,
            categoria.Descricao,
            transacao.PessoaId,
            pessoa.Nome
        ));
    }
}


using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Application.Interfaces;
using DesafioTecnico.Domain.Common;
using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Enums;
using DesafioTecnico.Domain.Interfaces;

namespace DesafioTecnico.Application.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PessoaService(
        IPessoaRepository pessoaRepository,
        ITransacaoRepository transacaoRepository,
        IUnitOfWork unitOfWork)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<PessoaDto>>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        var pessoas = await _pessoaRepository.ObterTodosAsync(cancellationToken);
        var dtos = pessoas.Select(p => new PessoaDto(p.Id, p.Nome, p.Idade));
        return Result.Success(dtos);
    }

    public async Task<Result<PessoaDto>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id, cancellationToken);
        if (pessoa is null)
            return Result.Failure<PessoaDto>(DomainErrors.Pessoa.NaoEncontrada);

        return Result.Success(new PessoaDto(pessoa.Id, pessoa.Nome, pessoa.Idade));
    }

    public async Task<Result<PessoaDto>> CriarAsync(CriarPessoaRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = ValidarPessoa(request.Nome, request.Idade);
        if (validationResult.IsFailure)
            return Result.Failure<PessoaDto>(validationResult.Error);

        var pessoa = Pessoa.Criar(request.Nome, request.Idade);
        await _pessoaRepository.AdicionarAsync(pessoa, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new PessoaDto(pessoa.Id, pessoa.Nome, pessoa.Idade));
    }

    public async Task<Result<PessoaDto>> AtualizarAsync(Guid id, AtualizarPessoaRequest request, CancellationToken cancellationToken = default)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id, cancellationToken);
        if (pessoa is null)
            return Result.Failure<PessoaDto>(DomainErrors.Pessoa.NaoEncontrada);

        var validationResult = ValidarPessoa(request.Nome, request.Idade);
        if (validationResult.IsFailure)
            return Result.Failure<PessoaDto>(validationResult.Error);

        pessoa.Atualizar(request.Nome, request.Idade);
        _pessoaRepository.Atualizar(pessoa);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new PessoaDto(pessoa.Id, pessoa.Nome, pessoa.Idade));
    }

    public async Task<Result> DeletarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id, cancellationToken);
        if (pessoa is null)
            return Result.Failure(DomainErrors.Pessoa.NaoEncontrada);

        _pessoaRepository.Remover(pessoa);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<TotaisGeralDto>> ObterTotaisPorPessoaAsync(CancellationToken cancellationToken = default)
    {
        var pessoas = await _pessoaRepository.ObterTodosAsync(cancellationToken);
        
        var totaisPorPessoa = pessoas.Select(p => new TotalPorPessoaDto(
            p.Id,
            p.Nome,
            p.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor),
            p.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor),
            p.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor) -
            p.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor)
        )).ToList();

        var totalGeralReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalGeralDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas);
        var saldoLiquido = totalGeralReceitas - totalGeralDespesas;

        return Result.Success(new TotaisGeralDto(totaisPorPessoa, totalGeralReceitas, totalGeralDespesas, saldoLiquido));
    }

    private static Result ValidarPessoa(string nome, int idade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure(DomainErrors.Pessoa.NomeVazio);

        if (nome.Length > 200)
            return Result.Failure(DomainErrors.Pessoa.NomeMuitoLongo);

        if (idade <= 0)
            return Result.Failure(DomainErrors.Pessoa.IdadeInvalida);

        return Result.Success();
    }
}


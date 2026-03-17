using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Domain.Common;

namespace DesafioTecnico.Application.Interfaces;

public interface IPessoaService
{
    Task<Result<IEnumerable<PessoaDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<Result<PessoaDto>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PessoaDto>> CriarAsync(CriarPessoaRequest request, CancellationToken cancellationToken = default);
    Task<Result<PessoaDto>> AtualizarAsync(Guid id, AtualizarPessoaRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeletarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<TotaisGeralDto>> ObterTotaisPorPessoaAsync(CancellationToken cancellationToken = default);
}


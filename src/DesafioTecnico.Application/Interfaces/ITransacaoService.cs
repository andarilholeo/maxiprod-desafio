using DesafioTecnico.Application.DTOs;
using DesafioTecnico.Domain.Common;

namespace DesafioTecnico.Application.Interfaces;

public interface ITransacaoService
{
    Task<Result<IEnumerable<TransacaoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<Result<TransacaoDto>> CriarAsync(CriarTransacaoRequest request, CancellationToken cancellationToken = default);
}


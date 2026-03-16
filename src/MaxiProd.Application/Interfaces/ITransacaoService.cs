using MaxiProd.Application.DTOs;
using MaxiProd.Domain.Common;

namespace MaxiProd.Application.Interfaces;

public interface ITransacaoService
{
    Task<Result<IEnumerable<TransacaoDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<Result<TransacaoDto>> CriarAsync(CriarTransacaoRequest request, CancellationToken cancellationToken = default);
}


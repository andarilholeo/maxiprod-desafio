using MaxiProd.Application.DTOs;
using MaxiProd.Domain.Common;

namespace MaxiProd.Application.Interfaces;

public interface ICategoriaService
{
    Task<Result<IEnumerable<CategoriaDto>>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<Result<CategoriaDto>> CriarAsync(CriarCategoriaRequest request, CancellationToken cancellationToken = default);
    Task<Result<TotaisCategoriasDto>> ObterTotaisPorCategoriaAsync(CancellationToken cancellationToken = default);
}


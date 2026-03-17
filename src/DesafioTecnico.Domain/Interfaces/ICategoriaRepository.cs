using DesafioTecnico.Domain.Entities;

namespace DesafioTecnico.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Categoria>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Categoria categoria, CancellationToken cancellationToken = default);
}


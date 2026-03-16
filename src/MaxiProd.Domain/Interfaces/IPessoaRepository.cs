using MaxiProd.Domain.Entities;

namespace MaxiProd.Domain.Interfaces;

public interface IPessoaRepository
{
    Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pessoa>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pessoa pessoa, CancellationToken cancellationToken = default);
    void Atualizar(Pessoa pessoa);
    void Remover(Pessoa pessoa);
}


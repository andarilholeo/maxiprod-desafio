using DesafioTecnico.Domain.Entities;

namespace DesafioTecnico.Domain.Interfaces;

public interface IPessoaRepository
{
    Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pessoa>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pessoa pessoa, CancellationToken cancellationToken = default);
    void Atualizar(Pessoa pessoa);
    void Remover(Pessoa pessoa);
}


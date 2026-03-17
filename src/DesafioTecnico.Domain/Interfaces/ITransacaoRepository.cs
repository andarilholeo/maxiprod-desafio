using DesafioTecnico.Domain.Entities;

namespace DesafioTecnico.Domain.Interfaces;

public interface ITransacaoRepository
{
    Task<Transacao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transacao>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Transacao>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default);
    void RemoverRange(IEnumerable<Transacao> transacoes);
}


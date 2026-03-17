using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Interfaces;
using DesafioTecnico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnico.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transacao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Transacao>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transacao>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes
            .Where(t => t.PessoaId == pessoaId)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default)
    {
        await _context.Transacoes.AddAsync(transacao, cancellationToken);
    }

    public void RemoverRange(IEnumerable<Transacao> transacoes)
    {
        _context.Transacoes.RemoveRange(transacoes);
    }
}


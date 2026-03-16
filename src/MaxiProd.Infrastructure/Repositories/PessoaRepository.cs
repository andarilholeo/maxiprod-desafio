using MaxiProd.Domain.Entities;
using MaxiProd.Domain.Interfaces;
using MaxiProd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaxiProd.Infrastructure.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas
            .Include(p => p.Transacoes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Pessoa>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas
            .Include(p => p.Transacoes)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Pessoa pessoa, CancellationToken cancellationToken = default)
    {
        await _context.Pessoas.AddAsync(pessoa, cancellationToken);
    }

    public void Atualizar(Pessoa pessoa)
    {
        _context.Pessoas.Update(pessoa);
    }

    public void Remover(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
    }
}


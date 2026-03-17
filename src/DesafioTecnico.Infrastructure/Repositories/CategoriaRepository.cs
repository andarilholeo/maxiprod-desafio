using DesafioTecnico.Domain.Entities;
using DesafioTecnico.Domain.Interfaces;
using DesafioTecnico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnico.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Categoria>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categorias
            .Include(c => c.Transacoes)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Categoria categoria, CancellationToken cancellationToken = default)
    {
        await _context.Categorias.AddAsync(categoria, cancellationToken);
    }
}


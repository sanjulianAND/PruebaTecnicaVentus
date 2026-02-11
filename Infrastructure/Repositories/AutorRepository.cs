using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly BibliotecaDbContext _context;

    public AutorRepository(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<Autor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Autores
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Autor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Autores
            .AsNoTracking()
            .OrderBy(a => a.NombreCompleto)
            .ToListAsync(cancellationToken);
    }

    public async Task<Autor> CreateAsync(Autor autor, CancellationToken cancellationToken = default)
    {
        autor.FechaCreacion = DateTime.UtcNow;
        autor.FechaActualizacion = DateTime.UtcNow;
        
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync(cancellationToken);
        return autor;
    }

    public async Task UpdateAsync(Autor autor, CancellationToken cancellationToken = default)
    {
        autor.FechaActualizacion = DateTime.UtcNow;
        _context.Autores.Update(autor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var autor = await _context.Autores.FindAsync(new object[] { id }, cancellationToken);
        if (autor != null)
        {
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Autores.AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Autores.AnyAsync(a => a.CorreoElectronico == email, cancellationToken);
    }
}

using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly BibliotecaDbContext _context;

    public LibroRepository(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<Libro?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Libros
            .AsNoTracking()
            .Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Libro>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Libros
            .AsNoTracking()
            .Include(l => l.Autor)
            .OrderBy(l => l.Titulo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Libro>> GetByAutorIdAsync(int autorId, CancellationToken cancellationToken = default)
    {
        return await _context.Libros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Where(l => l.AutorId == autorId)
            .OrderBy(l => l.Titulo)
            .ToListAsync(cancellationToken);
    }

    public async Task<Libro> CreateAsync(Libro libro, CancellationToken cancellationToken = default)
    {
        libro.FechaCreacion = DateTime.UtcNow;
        libro.FechaActualizacion = DateTime.UtcNow;
        
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync(cancellationToken);
        return libro;
    }

    public async Task UpdateAsync(Libro libro, CancellationToken cancellationToken = default)
    {
        libro.FechaActualizacion = DateTime.UtcNow;
        _context.Libros.Update(libro);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var libro = await _context.Libros.FindAsync(new object[] { id }, cancellationToken);
        if (libro != null)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Libros.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Libros.AnyAsync(l => l.Id == id, cancellationToken);
    }
}

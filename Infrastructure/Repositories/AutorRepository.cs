using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            .FirstOrDefaultAsync(a => a.Id == id && a.Activo, cancellationToken);
    }

    public async Task<IEnumerable<Autor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Autores
            .AsNoTracking()
            .Where(a => a.Activo)
            .OrderBy(a => a.NombreCompleto)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Autor> Items, int Total)> GetPaginadoAsync(int pagina, int tamanoPagina, string? ordenarPor, bool ordenDescendente, CancellationToken cancellationToken = default)
    {
        var query = _context.Autores.Where(a => a.Activo).AsNoTracking();

        if (!string.IsNullOrEmpty(ordenarPor))
        {
            var param = Expression.Parameter(typeof(Autor), "x");
            var property = Expression.Property(param, ordenarPor);
            var lambda = Expression.Lambda<Func<Autor, object>>(Expression.Convert(property, typeof(object)), param);

            query = ordenDescendente ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
        }
        else
        {
            query = query.OrderBy(a => a.NombreCompleto);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<Autor> CreateAsync(Autor autor, CancellationToken cancellationToken = default)
    {
        autor.FechaCreacion = DateTime.UtcNow;
        autor.FechaActualizacion = DateTime.UtcNow;
        autor.Activo = true;

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

    public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var autor = await _context.Autores.FindAsync(new object[] { id }, cancellationToken);
        if (autor != null)
        {
            autor.Activo = false;
            autor.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Autores.AnyAsync(a => a.Id == id && a.Activo, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Autores.AnyAsync(a => a.CorreoElectronico == email && a.Activo, cancellationToken);
    }
}

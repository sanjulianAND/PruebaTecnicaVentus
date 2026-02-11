using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly BibliotecaDbContext _context;

    public UsuarioRepository(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.Activo);
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.CorreoElectronico == correo && u.Activo);
    }

    public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Activo);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        usuario.FechaCreacion = DateTime.UtcNow;
        usuario.FechaActualizacion = DateTime.UtcNow;
        usuario.Activo = true;
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        usuario.FechaActualizacion = DateTime.UtcNow;
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }
}

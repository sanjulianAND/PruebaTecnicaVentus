using Domain.Entities;

namespace Infrastructure.Abstractions;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
}

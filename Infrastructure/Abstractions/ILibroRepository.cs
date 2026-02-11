using Domain.Entities;

namespace Infrastructure.Abstractions;

public interface ILibroRepository
{
    Task<Libro?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Libro>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Libro>> GetByAutorIdAsync(int autorId, CancellationToken cancellationToken = default);
    Task<Libro> CreateAsync(Libro libro, CancellationToken cancellationToken = default);
    Task UpdateAsync(Libro libro, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}

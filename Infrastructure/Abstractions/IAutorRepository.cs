using Domain.Entities;

namespace Infrastructure.Abstractions;

public interface IAutorRepository
{
    Task<Autor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Autor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Autor> CreateAsync(Autor autor, CancellationToken cancellationToken = default);
    Task UpdateAsync(Autor autor, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Libros.Delete;

public record EliminarLibroCommand(int Id) : IRequest<bool>;

public class EliminarLibroHandler : IRequestHandler<EliminarLibroCommand, bool>
{
    private readonly ILibroRepository _libroRepository;

    public EliminarLibroHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<bool> Handle(EliminarLibroCommand request, CancellationToken cancellationToken)
    {
        if (!await _libroRepository.ExistsAsync(request.Id, cancellationToken))
            return false;

        await _libroRepository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}

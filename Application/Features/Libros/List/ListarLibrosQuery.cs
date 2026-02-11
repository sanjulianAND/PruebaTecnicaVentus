using Application.DTOs;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Libros.List;

public record ListarLibrosQuery : IRequest<IEnumerable<LibroDto>>;

public class ListarLibrosHandler : IRequestHandler<ListarLibrosQuery, IEnumerable<LibroDto>>
{
    private readonly ILibroRepository _libroRepository;

    public ListarLibrosHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<IEnumerable<LibroDto>> Handle(ListarLibrosQuery request, CancellationToken cancellationToken)
    {
        var libros = await _libroRepository.GetAllAsync(cancellationToken);
        
        return libros.Select(l => new LibroDto
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Anio = l.Anio,
            Genero = l.Genero,
            NumeroPaginas = l.NumeroPaginas,
            AutorId = l.AutorId,
            AutorNombre = l.Autor?.NombreCompleto ?? string.Empty
        });
    }
}

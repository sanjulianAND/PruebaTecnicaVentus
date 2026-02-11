using Application.DTOs;
using Domain.Exceptions;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Libros.Update;

public record ActualizarLibroCommand(ActualizarLibroDto Datos) : IRequest<LibroDto>;

public class ActualizarLibroHandler : IRequestHandler<ActualizarLibroCommand, LibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly IAutorRepository _autorRepository;

    public ActualizarLibroHandler(
        ILibroRepository libroRepository, 
        IAutorRepository autorRepository)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;
    }

    public async Task<LibroDto> Handle(ActualizarLibroCommand request, CancellationToken cancellationToken)
    {
        var libro = await _libroRepository.GetByIdAsync(request.Datos.Id, cancellationToken);
        
        if (libro == null)
            throw new LibroNoEncontradoException(request.Datos.Id);

        // Verificar que el autor existe
        if (!await _autorRepository.ExistsAsync(request.Datos.AutorId, cancellationToken))
            throw new AutorNoEncontradoException();

        libro.Titulo = request.Datos.Titulo;
        libro.Anio = request.Datos.Anio;
        libro.Genero = request.Datos.Genero;
        libro.NumeroPaginas = request.Datos.NumeroPaginas;
        libro.AutorId = request.Datos.AutorId;

        await _libroRepository.UpdateAsync(libro, cancellationToken);

        return new LibroDto
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Anio = libro.Anio,
            Genero = libro.Genero,
            NumeroPaginas = libro.NumeroPaginas,
            AutorId = libro.AutorId
        };
    }
}

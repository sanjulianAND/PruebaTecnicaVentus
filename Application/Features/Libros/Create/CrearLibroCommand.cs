using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Libros.Create;

public record CrearLibroCommand(CrearLibroDto Datos) : IRequest<LibroDto>;

public class CrearLibroHandler : IRequestHandler<CrearLibroCommand, LibroDto>
{
    private readonly ILibroRepository _libroRepository;
    private readonly IAutorRepository _autorRepository;
    private readonly int _maximoLibrosPermitidos;

    public CrearLibroHandler(
        ILibroRepository libroRepository, 
        IAutorRepository autorRepository,
        IConfiguration configuration)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;
        _maximoLibrosPermitidos = configuration.GetValue<int>("MaximoLibrosPermitidos", 100);
    }

    public async Task<LibroDto> Handle(CrearLibroCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el autor existe
        if (!await _autorRepository.ExistsAsync(request.Datos.AutorId, cancellationToken))
            throw new AutorNoEncontradoException();

        // Verificar límite de libros
        var totalLibros = await _libroRepository.CountAsync(cancellationToken);
        if (totalLibros >= _maximoLibrosPermitidos)
            throw new MaximoLibrosException();

        var libro = new Libro
        {
            Titulo = request.Datos.Titulo,
            Anio = request.Datos.Anio,
            Genero = request.Datos.Genero,
            NumeroPaginas = request.Datos.NumeroPaginas,
            AutorId = request.Datos.AutorId
        };

        var libroCreado = await _libroRepository.CreateAsync(libro, cancellationToken);

        return new LibroDto
        {
            Id = libroCreado.Id,
            Titulo = libroCreado.Titulo,
            Anio = libroCreado.Anio,
            Genero = libroCreado.Genero,
            NumeroPaginas = libroCreado.NumeroPaginas,
            AutorId = libroCreado.AutorId
        };
    }
}

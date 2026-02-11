using Application.DTOs;
using Domain.Entities;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Autores.Create;

public record CrearAutorCommand(CrearAutorDto Datos) : IRequest<AutorDto>;

public class CrearAutorHandler : IRequestHandler<CrearAutorCommand, AutorDto>
{
    private readonly IAutorRepository _autorRepository;

    public CrearAutorHandler(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<AutorDto> Handle(CrearAutorCommand request, CancellationToken cancellationToken)
    {
        var autor = new Autor
        {
            NombreCompleto = request.Datos.NombreCompleto,
            FechaNacimiento = request.Datos.FechaNacimiento,
            CiudadProcedencia = request.Datos.CiudadProcedencia,
            CorreoElectronico = request.Datos.CorreoElectronico
        };

        var autorCreado = await _autorRepository.CreateAsync(autor, cancellationToken);

        return new AutorDto
        {
            Id = autorCreado.Id,
            NombreCompleto = autorCreado.NombreCompleto,
            FechaNacimiento = autorCreado.FechaNacimiento,
            CiudadProcedencia = autorCreado.CiudadProcedencia,
            CorreoElectronico = autorCreado.CorreoElectronico
        };
    }
}

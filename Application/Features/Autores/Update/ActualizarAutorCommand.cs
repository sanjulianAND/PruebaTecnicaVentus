using Application.DTOs;
using Domain.Exceptions;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Autores.Update;

public record ActualizarAutorCommand(ActualizarAutorDto Datos) : IRequest<AutorDto>;

public class ActualizarAutorHandler : IRequestHandler<ActualizarAutorCommand, AutorDto>
{
    private readonly IAutorRepository _autorRepository;

    public ActualizarAutorHandler(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<AutorDto> Handle(ActualizarAutorCommand request, CancellationToken cancellationToken)
    {
        var autor = await _autorRepository.GetByIdAsync(request.Datos.Id, cancellationToken);
        
        if (autor == null)
            throw new AutorNoEncontradoException();

        autor.NombreCompleto = request.Datos.NombreCompleto;
        autor.FechaNacimiento = request.Datos.FechaNacimiento;
        autor.CiudadProcedencia = request.Datos.CiudadProcedencia;
        autor.CorreoElectronico = request.Datos.CorreoElectronico;

        await _autorRepository.UpdateAsync(autor, cancellationToken);

        return new AutorDto
        {
            Id = autor.Id,
            NombreCompleto = autor.NombreCompleto,
            FechaNacimiento = autor.FechaNacimiento,
            CiudadProcedencia = autor.CiudadProcedencia,
            CorreoElectronico = autor.CorreoElectronico
        };
    }
}

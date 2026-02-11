using Application.DTOs;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Autores.List;

public record ListarAutoresQuery : IRequest<IEnumerable<AutorDto>>;

public class ListarAutoresHandler : IRequestHandler<ListarAutoresQuery, IEnumerable<AutorDto>>
{
    private readonly IAutorRepository _autorRepository;

    public ListarAutoresHandler(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<IEnumerable<AutorDto>> Handle(ListarAutoresQuery request, CancellationToken cancellationToken)
    {
        var autores = await _autorRepository.GetAllAsync(cancellationToken);
        
        return autores.Select(a => new AutorDto
        {
            Id = a.Id,
            NombreCompleto = a.NombreCompleto,
            FechaNacimiento = a.FechaNacimiento,
            CiudadProcedencia = a.CiudadProcedencia,
            CorreoElectronico = a.CorreoElectronico
        });
    }
}

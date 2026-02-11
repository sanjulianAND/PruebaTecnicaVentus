using Application.DTOs;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Features.Autores.List;

public record ListarAutoresPaginadoQuery(ParametrosPaginacion Parametros) : IRequest<PaginacionDto<AutorDto>>;

public class ListarAutoresPaginadoHandler : IRequestHandler<ListarAutoresPaginadoQuery, PaginacionDto<AutorDto>>
{
    private readonly IAutorRepository _autorRepository;

    public ListarAutoresPaginadoHandler(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<PaginacionDto<AutorDto>> Handle(ListarAutoresPaginadoQuery request, CancellationToken cancellationToken)
    {
        var (autores, totalItems) = await _autorRepository.GetPaginadoAsync(
            request.Parametros.Pagina, 
            request.Parametros.TamanoPagina,
            request.Parametros.OrdenarPor,
            request.Parametros.OrdenDescendente,
            cancellationToken);

        var items = autores.Select(a => new AutorDto
        {
            Id = a.Id,
            NombreCompleto = a.NombreCompleto,
            FechaNacimiento = a.FechaNacimiento,
            CiudadProcedencia = a.CiudadProcedencia,
            CorreoElectronico = a.CorreoElectronico
        }).ToList();

        var totalPaginas = (int)Math.Ceiling(totalItems / (double)request.Parametros.TamanoPagina);

        return new PaginacionDto<AutorDto>
        {
            Items = items,
            TotalItems = totalItems,
            PaginaActual = request.Parametros.Pagina,
            TotalPaginas = totalPaginas,
            TamanoPagina = request.Parametros.TamanoPagina
        };
    }
}

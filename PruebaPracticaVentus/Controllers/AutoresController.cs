using Application.DTOs;
using Application.Features.Autores.Create;
using Application.Features.Autores.Delete;
using Application.Features.Autores.List;
using Application.Features.Autores.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebaPracticaVentus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AutoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public AutoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<RespuestaApi<IEnumerable<AutorDto>>>> GetAll()
    {
        var query = new ListarAutoresQuery();
        var autores = await _mediator.Send(query);
        return Ok(RespuestaApi<IEnumerable<AutorDto>>.Exitosa(autores));
    }

    [HttpGet("paginado")]
    [AllowAnonymous]
    public async Task<ActionResult<RespuestaApi<PaginacionDto<AutorDto>>>> GetPaginado([FromQuery] ParametrosPaginacion parametros)
    {
        var query = new ListarAutoresPaginadoQuery(parametros);
        var resultado = await _mediator.Send(query);
        return Ok(RespuestaApi<PaginacionDto<AutorDto>>.Exitosa(resultado));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Usuario")]
    public async Task<ActionResult<RespuestaApi<AutorDto>>> Create([FromBody] CrearAutorDto dto)
    {
        var command = new CrearAutorCommand(dto);
        var autor = await _mediator.Send(command);
        return Ok(RespuestaApi<AutorDto>.Exitosa(autor, "Autor creado exitosamente"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador,Usuario")]
    public async Task<ActionResult<RespuestaApi<AutorDto>>> Update(int id, [FromBody] ActualizarAutorDto dto)
    {
        if (id != dto.Id)
            return BadRequest(RespuestaApi<AutorDto>.Error("El ID de la URL no coincide con el ID del cuerpo"));

        var command = new ActualizarAutorCommand(dto);
        var autor = await _mediator.Send(command);
        return Ok(RespuestaApi<AutorDto>.Exitosa(autor, "Autor actualizado exitosamente"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<RespuestaApi<bool>>> Delete(int id)
    {
        var command = new EliminarAutorCommand(id);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return NotFound(RespuestaApi<bool>.Error("Autor no encontrado"));

        return Ok(RespuestaApi<bool>.Exitosa(true, "Autor eliminado exitosamente"));
    }
}

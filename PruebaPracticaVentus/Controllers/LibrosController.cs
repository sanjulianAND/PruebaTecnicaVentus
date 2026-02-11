using Application.DTOs;
using Application.Features.Libros.Create;
using Application.Features.Libros.Delete;
using Application.Features.Libros.List;
using Application.Features.Libros.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebaPracticaVentus.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LibrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public LibrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<RespuestaApi<IEnumerable<LibroDto>>>> GetAll()
    {
        var query = new ListarLibrosQuery();
        var libros = await _mediator.Send(query);
        return Ok(RespuestaApi<IEnumerable<LibroDto>>.Exitosa(libros));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Usuario")]
    public async Task<ActionResult<RespuestaApi<LibroDto>>> Create([FromBody] CrearLibroDto dto)
    {
        var command = new CrearLibroCommand(dto);
        var libro = await _mediator.Send(command);
        return Ok(RespuestaApi<LibroDto>.Exitosa(libro, "Libro creado exitosamente"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador,Usuario")]
    public async Task<ActionResult<RespuestaApi<LibroDto>>> Update(int id, [FromBody] ActualizarLibroDto dto)
    {
        if (id != dto.Id)
            return BadRequest(RespuestaApi<LibroDto>.Error("El ID de la URL no coincide con el ID del cuerpo"));

        var command = new ActualizarLibroCommand(dto);
        var libro = await _mediator.Send(command);
        return Ok(RespuestaApi<LibroDto>.Exitosa(libro, "Libro actualizado exitosamente"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<RespuestaApi<bool>>> Delete(int id)
    {
        var command = new EliminarLibroCommand(id);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return NotFound(RespuestaApi<bool>.Error("Libro no encontrado"));

        return Ok(RespuestaApi<bool>.Exitosa(true, "Libro eliminado exitosamente"));
    }
}

using Application.DTOs;
using Application.Features.Libros.Create;
using Application.Features.Libros.Delete;
using Application.Features.Libros.List;
using Application.Features.Libros.Update;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaPracticaVentus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public LibrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<RespuestaApi<IEnumerable<LibroDto>>>> GetAll()
    {
        try
        {
            var query = new ListarLibrosQuery();
            var libros = await _mediator.Send(query);
            return Ok(RespuestaApi<IEnumerable<LibroDto>>.Exitosa(libros));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<IEnumerable<LibroDto>>.Error(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaApi<LibroDto>>> Create([FromBody] CrearLibroDto dto)
    {
        try
        {
            var command = new CrearLibroCommand(dto);
            var libro = await _mediator.Send(command);
            return Ok(RespuestaApi<LibroDto>.Exitosa(libro, "Libro creado exitosamente"));
        }
        catch (AutorNoEncontradoException)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error("El autor no está registrado"));
        }
        catch (MaximoLibrosException ex)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error(ex.Message));
        }
        catch (ValidacionException ex)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error("Error de validación", ex.Errores));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RespuestaApi<LibroDto>>> Update(int id, [FromBody] ActualizarLibroDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(RespuestaApi<LibroDto>.Error("El ID de la URL no coincide con el ID del cuerpo"));

            var command = new ActualizarLibroCommand(dto);
            var libro = await _mediator.Send(command);
            return Ok(RespuestaApi<LibroDto>.Exitosa(libro, "Libro actualizado exitosamente"));
        }
        catch (LibroNoEncontradoException ex)
        {
            return NotFound(RespuestaApi<LibroDto>.Error(ex.Message));
        }
        catch (AutorNoEncontradoException)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error("El autor no está registrado"));
        }
        catch (ValidacionException ex)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error("Error de validación", ex.Errores));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<LibroDto>.Error(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RespuestaApi<bool>>> Delete(int id)
    {
        try
        {
            var command = new EliminarLibroCommand(id);
            var resultado = await _mediator.Send(command);
            
            if (!resultado)
                return NotFound(RespuestaApi<bool>.Error("Libro no encontrado"));

            return Ok(RespuestaApi<bool>.Exitosa(true, "Libro eliminado exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<bool>.Error(ex.Message));
        }
    }
}

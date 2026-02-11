using Application.DTOs;
using Application.Features.Autores.Create;
using Application.Features.Autores.Delete;
using Application.Features.Autores.List;
using Application.Features.Autores.Update;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaPracticaVentus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public AutoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<RespuestaApi<IEnumerable<AutorDto>>>> GetAll()
    {
        try
        {
            var query = new ListarAutoresQuery();
            var autores = await _mediator.Send(query);
            return Ok(RespuestaApi<IEnumerable<AutorDto>>.Exitosa(autores));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<IEnumerable<AutorDto>>.Error(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaApi<AutorDto>>> Create([FromBody] CrearAutorDto dto)
    {
        try
        {
            var command = new CrearAutorCommand(dto);
            var autor = await _mediator.Send(command);
            return Ok(RespuestaApi<AutorDto>.Exitosa(autor, "Autor creado exitosamente"));
        }
        catch (ValidacionException ex)
        {
            return BadRequest(RespuestaApi<AutorDto>.Error("Error de validación", ex.Errores));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<AutorDto>.Error(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RespuestaApi<AutorDto>>> Update(int id, [FromBody] ActualizarAutorDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(RespuestaApi<AutorDto>.Error("El ID de la URL no coincide con el ID del cuerpo"));

            var command = new ActualizarAutorCommand(dto);
            var autor = await _mediator.Send(command);
            return Ok(RespuestaApi<AutorDto>.Exitosa(autor, "Autor actualizado exitosamente"));
        }
        catch (AutorNoEncontradoException)
        {
            return NotFound(RespuestaApi<AutorDto>.Error("El autor no está registrado"));
        }
        catch (ValidacionException ex)
        {
            return BadRequest(RespuestaApi<AutorDto>.Error("Error de validación", ex.Errores));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<AutorDto>.Error(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RespuestaApi<bool>>> Delete(int id)
    {
        try
        {
            var command = new EliminarAutorCommand(id);
            var resultado = await _mediator.Send(command);
            
            if (!resultado)
                return NotFound(RespuestaApi<bool>.Error("Autor no encontrado"));

            return Ok(RespuestaApi<bool>.Exitosa(true, "Autor eliminado exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(RespuestaApi<bool>.Error(ex.Message));
        }
    }
}

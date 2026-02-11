using Application.DTOs;
using Application.Features.Auth.Login;
using Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaPracticaVentus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<RespuestaApi<AuthResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto);
        var resultado = await _mediator.Send(command);
        return Ok(RespuestaApi<AuthResponseDto>.Exitosa(resultado, "Inicio de sesión exitoso"));
    }

    [HttpPost("register")]
    public async Task<ActionResult<RespuestaApi<AuthResponseDto>>> Register([FromBody] RegistroDto dto)
    {
        var command = new RegisterCommand(dto);
        var resultado = await _mediator.Send(command);
        return Ok(RespuestaApi<AuthResponseDto>.Exitosa(resultado, "Usuario registrado exitosamente"));
    }
}

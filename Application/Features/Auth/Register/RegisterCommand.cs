using Application.DTOs;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Services;
using MediatR;

namespace Application.Features.Auth.Register;

public record RegisterCommand(RegistroDto Datos) : IRequest<AuthResponseDto>;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtService _jwtService;

    public RegisterHandler(IUsuarioRepository usuarioRepository, IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var usuarioExistente = await _usuarioRepository.GetByCorreoAsync(request.Datos.CorreoElectronico);
        if (usuarioExistente != null)
            throw new InvalidOperationException("El correo electrónico ya está registrado");

        var usuarioNombreExistente = await _usuarioRepository.GetByNombreUsuarioAsync(request.Datos.NombreUsuario);
        if (usuarioNombreExistente != null)
            throw new InvalidOperationException("El nombre de usuario ya está registrado");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Datos.Password);

        var usuario = new Usuario
        {
            NombreUsuario = request.Datos.NombreUsuario,
            CorreoElectronico = request.Datos.CorreoElectronico,
            PasswordHash = passwordHash,
            Rol = "Usuario"
        };

        var usuarioCreado = await _usuarioRepository.CreateAsync(usuario);

        var token = _jwtService.GenerarToken(usuarioCreado.Id, usuarioCreado.NombreUsuario, usuarioCreado.CorreoElectronico, usuarioCreado.Rol);
        var refreshToken = _jwtService.GenerarRefreshToken();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiracion = DateTime.Now.AddHours(2),
            Usuario = new UsuarioDto
            {
                Id = usuarioCreado.Id,
                NombreUsuario = usuarioCreado.NombreUsuario,
                CorreoElectronico = usuarioCreado.CorreoElectronico,
                Rol = usuarioCreado.Rol
            }
        };
    }
}

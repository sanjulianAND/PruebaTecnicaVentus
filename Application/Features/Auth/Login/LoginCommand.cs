using Application.DTOs;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Services;
using MediatR;

namespace Application.Features.Auth.Login;

public record LoginCommand(LoginDto Datos) : IRequest<AuthResponseDto>;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtService _jwtService;

    public LoginHandler(IUsuarioRepository usuarioRepository, IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(request.Datos.CorreoElectronico);

        if (usuario == null)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        if (!BCrypt.Net.BCrypt.Verify(request.Datos.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var token = _jwtService.GenerarToken(usuario.Id, usuario.NombreUsuario, usuario.CorreoElectronico, usuario.Rol);
        var refreshToken = _jwtService.GenerarRefreshToken();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiracion = DateTime.Now.AddHours(2),
            Usuario = new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                CorreoElectronico = usuario.CorreoElectronico,
                Rol = usuario.Rol
            }
        };
    }
}

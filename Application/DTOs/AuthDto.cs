namespace Application.DTOs;

public class LoginDto
{
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegistroDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
    public UsuarioDto Usuario { get; set; } = new UsuarioDto();
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

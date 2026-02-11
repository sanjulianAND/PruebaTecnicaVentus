namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Usuario";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public bool Activo { get; set; } = true;
}

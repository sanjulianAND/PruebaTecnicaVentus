namespace Application.DTOs;

public class AutorDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string CiudadProcedencia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
}

public class CrearAutorDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string CiudadProcedencia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
}

public class ActualizarAutorDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string CiudadProcedencia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
}

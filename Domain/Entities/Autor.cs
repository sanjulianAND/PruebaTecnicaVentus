namespace Domain.Entities;

public class Autor
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string CiudadProcedencia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    
    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}

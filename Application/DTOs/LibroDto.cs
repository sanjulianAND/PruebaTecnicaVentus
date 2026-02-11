namespace Application.DTOs;

public class LibroDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Genero { get; set; } = string.Empty;
    public int NumeroPaginas { get; set; }
    public int AutorId { get; set; }
    public string AutorNombre { get; set; } = string.Empty;
}

public class CrearLibroDto
{
    public string Titulo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Genero { get; set; } = string.Empty;
    public int NumeroPaginas { get; set; }
    public int AutorId { get; set; }
}

public class ActualizarLibroDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Genero { get; set; } = string.Empty;
    public int NumeroPaginas { get; set; }
    public int AutorId { get; set; }
}

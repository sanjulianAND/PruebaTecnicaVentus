namespace Application.DTOs;

public class PaginacionDto<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalItems { get; set; }
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TamanoPagina { get; set; }
    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
}

public class ParametrosPaginacion
{
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;
    public string? OrdenarPor { get; set; }
    public bool OrdenDescendente { get; set; } = false;
}

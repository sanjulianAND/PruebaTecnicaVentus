namespace Application.DTOs;

public class RespuestaApi<T>
{
    public bool Exito { get; set; }
    public string? Mensaje { get; set; }
    public T? Datos { get; set; }
    public List<string>? Errores { get; set; }

    public static RespuestaApi<T> Exitosa(T datos, string? mensaje = null)
    {
        return new RespuestaApi<T>
        {
            Exito = true,
            Mensaje = mensaje,
            Datos = datos
        };
    }

    public static RespuestaApi<T> Error(string mensaje, List<string>? errores = null)
    {
        return new RespuestaApi<T>
        {
            Exito = false,
            Mensaje = mensaje,
            Errores = errores
        };
    }
}

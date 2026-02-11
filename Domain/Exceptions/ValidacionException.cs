namespace Domain.Exceptions;

public class ValidacionException : Exception
{
    public List<string> Errores { get; }

    public ValidacionException(List<string> errores) 
        : base("Error de validación")
    {
        Errores = errores;
    }
}

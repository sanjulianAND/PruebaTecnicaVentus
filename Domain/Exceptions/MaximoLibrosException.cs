namespace Domain.Exceptions;

public class MaximoLibrosException : Exception
{
    public MaximoLibrosException() 
        : base("No es posible registrar el libro, se alcanzó el máximo permitido.")
    {
    }
}

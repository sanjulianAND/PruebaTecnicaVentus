namespace Domain.Exceptions;

public class AutorNoEncontradoException : Exception
{
    public AutorNoEncontradoException() 
        : base("El autor no está registrado")
    {
    }
}

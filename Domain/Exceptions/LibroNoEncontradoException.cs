namespace Domain.Exceptions;

public class LibroNoEncontradoException : Exception
{
    public LibroNoEncontradoException(int id) 
        : base($"No se encontró el libro con ID: {id}")
    {
    }
}

namespace ApiInnovacion.Excepciones;

public class NoEncontradoExcepcion : Exception
{
    public NoEncontradoExcepcion(string mensaje) : base(mensaje)
    {
    }

    public NoEncontradoExcepcion(string entidad, object id)
        : base($"No se encontro el recurso '{entidad}' con identificador '{id}' o se encuentra inactivo.")
    {
    }
}
namespace ApiInnovacion.Excepciones;

public class ConflictoExcepcion : Exception
{
    public ConflictoExcepcion(string mensaje) : base(mensaje)
    {
    }
}
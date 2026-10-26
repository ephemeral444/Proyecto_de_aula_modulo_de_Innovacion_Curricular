using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioAcreditacion
{
    Task<IEnumerable<Acreditacion>> ObtenerTodosAsync();
    Task<Acreditacion?> ObtenerPorResolucionAsync(string resolucion);
    Task<Acreditacion> CrearAsync(Acreditacion acreditacion);
    Task<Acreditacion?> ActualizarAsync(Acreditacion acreditacion);
    Task<bool> EliminarAsync(string resolucion);
}
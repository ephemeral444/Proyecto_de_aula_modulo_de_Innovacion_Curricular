using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioPrograma
{
    Task<IEnumerable<Programa>> ObtenerTodosAsync();
    Task<Programa> ObtenerPorIdAsync(int id);
    Task<Programa> CrearAsync(ProgramaCrear peticion);
    Task<Programa> ActualizarAsync(int id, ProgramaActualizar peticion);
    Task EliminarAsync(int id);
}
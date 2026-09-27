using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioEnfoque
{
    Task<IEnumerable<Enfoque>> ObtenerTodosAsync();
    Task<Enfoque> ObtenerPorIdAsync(int id);
    Task<Enfoque> CrearAsync(EnfoqueCrear peticion);
    Task<Enfoque> ReemplazarAsync(int id, EnfoqueReemplazo peticion);
    Task<Enfoque> ActualizarAsync(int id, EnfoqueActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
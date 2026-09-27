using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioUniversidad
{
    Task<IEnumerable<Universidad>> ObtenerTodosAsync();
    Task<Universidad> ObtenerPorIdAsync(int id);
    Task<Universidad> CrearAsync(UniversidadCrear peticion);
    Task<Universidad> ReemplazarAsync(int id, UniversidadReemplazo peticion);
    Task<Universidad> ActualizarAsync(int id, UniversidadActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
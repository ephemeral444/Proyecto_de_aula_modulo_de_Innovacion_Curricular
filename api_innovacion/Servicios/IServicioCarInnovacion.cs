using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioCarInnovacion
{
    Task<IEnumerable<CarInnovacion>> ObtenerTodosAsync();
    Task<CarInnovacion> ObtenerPorIdAsync(int id);
    Task<CarInnovacion> CrearAsync(CarInnovacionCrear peticion);
    Task<CarInnovacion> ReemplazarAsync(int id, CarInnovacionReemplazo peticion);
    Task<CarInnovacion> ActualizarAsync(int id, CarInnovacionActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioAreaConocimiento
{
    Task<IEnumerable<AreaConocimiento>> ObtenerTodosAsync();
    Task<AreaConocimiento> ObtenerPorIdAsync(int id);
    Task<AreaConocimiento> CrearAsync(AreaConocimientoCrear peticion);
    Task<AreaConocimiento> ReemplazarAsync(int id, AreaConocimientoReemplazo peticion);
    Task<AreaConocimiento> ActualizarAsync(int id, AreaConocimientoActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
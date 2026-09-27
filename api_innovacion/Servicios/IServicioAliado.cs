using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioAliado
{
    Task<IEnumerable<Aliado>> ObtenerTodosAsync();
    Task<Aliado> ObtenerPorNitAsync(string nit);
    Task<Aliado> CrearAsync(AliadoCrear peticion);
    Task<Aliado> ReemplazarAsync(string nit, AliadoReemplazo peticion);
    Task<Aliado> ActualizarAsync(string nit, AliadoActualizar peticion);
    Task EliminarLogicoAsync(string nit);
}
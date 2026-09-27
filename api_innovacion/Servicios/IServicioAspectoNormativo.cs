using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioAspectoNormativo
{
    Task<IEnumerable<AspectoNormativo>> ObtenerTodosAsync();
    Task<AspectoNormativo> ObtenerPorIdAsync(int id);
    Task<AspectoNormativo> CrearAsync(AspectoNormativoCrear peticion);
    Task<AspectoNormativo> ReemplazarAsync(int id, AspectoNormativoReemplazo peticion);
    Task<AspectoNormativo> ActualizarAsync(int id, AspectoNormativoActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
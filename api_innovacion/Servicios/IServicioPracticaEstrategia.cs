using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioPracticaEstrategia
{
    Task<IEnumerable<PracticaEstrategia>> ObtenerTodosAsync();
    Task<PracticaEstrategia> ObtenerPorIdAsync(int id);
    Task<PracticaEstrategia> CrearAsync(PracticaEstrategiaCrear peticion);
    Task<PracticaEstrategia> ReemplazarAsync(int id, PracticaEstrategiaReemplazo peticion);
    Task<PracticaEstrategia> ActualizarAsync(int id, PracticaEstrategiaActualizar peticion);
    Task EliminarLogicoAsync(int id);
}
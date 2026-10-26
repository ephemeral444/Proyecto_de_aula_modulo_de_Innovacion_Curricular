using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioPremio
{
	Task<IEnumerable<Premio>> ObtenerTodosAsync();
	Task<Premio> CrearAsync(PremioCrear peticion);
    Task<Premio> ActualizarAsync(int id, PremioActualizar peticion);
    Task EliminarAsync(int id);
}
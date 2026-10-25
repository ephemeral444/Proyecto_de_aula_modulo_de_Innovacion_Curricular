using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioPasantia
{
    Task<IEnumerable<Pasantia>> ObtenerTodosAsync();
    Task<Pasantia> CrearAsync(PasantiaCrear peticion);
    Task<Pasantia> ActualizarAsync(int id, PasantiaActualizar peticion);
    Task EliminarAsync(int id);
}
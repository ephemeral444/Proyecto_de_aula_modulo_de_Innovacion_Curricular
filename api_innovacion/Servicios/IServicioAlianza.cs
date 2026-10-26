using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioAlianza
{
    Task<IEnumerable<Alianza>> ObtenerTodosAsync();
    Task<Alianza> CrearAsync(AlianzaCrear peticion);
    Task<Alianza> ActualizarAsync(int id, AlianzaActualizar peticion);
    Task EliminarAsync(int id);
}
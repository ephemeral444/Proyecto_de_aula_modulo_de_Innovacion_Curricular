using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioAcreditacion
{
    Task<IEnumerable<Acreditacion>> ObtenerTodosAsync();
    Task<Acreditacion> CrearAsync(AcreditacionCrear peticion);
    Task<Acreditacion> ActualizarAsync(string resolucion, AcreditacionActualizar peticion);
    Task EliminarAsync(string resolucion);
}
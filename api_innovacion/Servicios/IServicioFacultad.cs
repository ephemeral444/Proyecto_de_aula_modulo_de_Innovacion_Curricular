using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioFacultad
{
    Task<IEnumerable<Facultad>> ObtenerTodosAsync();
    Task<Facultad> ObtenerPorIdAsync(int id);
    Task<Facultad> CrearAsync(FacultadCrear peticion);
    Task<Facultad> ActualizarAsync(int id, FacultadActualizar peticion);
    Task EliminarAsync(int id);
}
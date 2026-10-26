using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioRegistroCalificado
{
    Task<IEnumerable<RegistroCalificado>> ObtenerTodosAsync();
    Task<RegistroCalificado> CrearAsync(RegistroCalificadoCrear peticion);
    Task<RegistroCalificado> ActualizarAsync(string codigo, RegistroCalificadoActualizar peticion);
    Task EliminarAsync(string codigo);
}
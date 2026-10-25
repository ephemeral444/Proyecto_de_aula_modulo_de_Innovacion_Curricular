using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioActivAcademica
{
    Task<IEnumerable<ActivAcademica>> ObtenerTodosAsync();
    Task<ActivAcademica> CrearAsync(ActivAcademicaCrear peticion);
    Task<ActivAcademica> ActualizarAsync(int id, ActivAcademicaActualizar peticion);
    Task EliminarAsync(int id);
}
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;

namespace ApiInnovacion.Servicios;

public interface IServicioDocenteDepartamento
{
    Task<IEnumerable<DocenteDepartamento>> ObtenerTodosAsync();
    Task<DocenteDepartamento> CrearAsync(DocenteDepartamentoCrear peticion);
    Task<DocenteDepartamento> ActualizarAsync(int id, DocenteDepartamentoActualizar peticion);
    Task EliminarAsync(int id);
}
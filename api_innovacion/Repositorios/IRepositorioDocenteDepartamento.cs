using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioDocenteDepartamento
{
    Task<IEnumerable<DocenteDepartamento>> ObtenerTodosAsync();
    Task<DocenteDepartamento> CrearAsync(DocenteDepartamento entidad);
    Task<DocenteDepartamento?> ActualizarAsync(DocenteDepartamento entidad);
    Task<bool> EliminarAsync(int id);
}
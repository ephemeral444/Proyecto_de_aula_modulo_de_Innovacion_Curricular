using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioPremio
{
    Task<IEnumerable<Premio>> ObtenerTodosAsync();
    Task<Premio> CrearAsync(Premio entidad);
    Task<Premio?> ActualizarAsync(Premio entidad);
    Task<bool> EliminarAsync(int id);
}
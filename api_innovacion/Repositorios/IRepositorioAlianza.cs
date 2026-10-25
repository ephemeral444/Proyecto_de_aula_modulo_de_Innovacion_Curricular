using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioAlianza
{
    Task<IEnumerable<Alianza>> ObtenerTodosAsync();
    Task<Alianza> CrearAsync(Alianza entidad);
    Task<Alianza?> ActualizarAsync(Alianza entidad);
    Task<bool> EliminarAsync(int id);
}
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioPrograma
{
    Task<IEnumerable<Programa>> ObtenerTodosAsync();
    Task<Programa?> ObtenerPorIdAsync(int id);
    Task<Programa> CrearAsync(Programa programa);
    Task<Programa?> ActualizarAsync(Programa programa);
    Task<bool> EliminarAsync(int id);
}
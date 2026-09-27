using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioEnfoque
{
    Task<List<Enfoque>> ObtenerTodosAsync();
    Task<Enfoque?> ObtenerPorIdAsync(int id);
    Task<Enfoque> CrearAsync(Enfoque entidad);
    Task<bool> ActualizarAsync(Enfoque entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
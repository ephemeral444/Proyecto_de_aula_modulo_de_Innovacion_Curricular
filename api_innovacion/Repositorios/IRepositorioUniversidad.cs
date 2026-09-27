using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioUniversidad
{
    Task<List<Universidad>> ObtenerTodosAsync();
    Task<Universidad?> ObtenerPorIdAsync(int id);
    Task<Universidad> CrearAsync(Universidad entidad);
    Task<bool> ActualizarAsync(Universidad entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
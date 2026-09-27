using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioAreaConocimiento
{
    Task<List<AreaConocimiento>> ObtenerTodosAsync();
    Task<AreaConocimiento?> ObtenerPorIdAsync(int id);
    Task<AreaConocimiento> CrearAsync(AreaConocimiento entidad);
    Task<bool> ActualizarAsync(AreaConocimiento entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
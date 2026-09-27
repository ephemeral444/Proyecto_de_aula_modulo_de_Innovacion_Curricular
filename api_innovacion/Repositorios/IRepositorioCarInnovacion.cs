using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioCarInnovacion
{
    Task<List<CarInnovacion>> ObtenerTodosAsync();
    Task<CarInnovacion?> ObtenerPorIdAsync(int id);
    Task<CarInnovacion> CrearAsync(CarInnovacion entidad);
    Task<bool> ActualizarAsync(CarInnovacion entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
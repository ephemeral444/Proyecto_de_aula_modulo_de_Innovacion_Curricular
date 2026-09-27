using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioAliado
{
    Task<List<Aliado>> ObtenerTodosAsync();
    Task<Aliado?> ObtenerPorNitAsync(string nit);
    Task<Aliado> CrearAsync(Aliado entidad);
    Task<bool> ActualizarAsync(Aliado entidad);
    Task<bool> BorradoLogicoAsync(string nit);
}
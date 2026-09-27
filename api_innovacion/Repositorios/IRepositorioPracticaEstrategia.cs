using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioPracticaEstrategia
{
    Task<List<PracticaEstrategia>> ObtenerTodosAsync();
    Task<PracticaEstrategia?> ObtenerPorIdAsync(int id);
    Task<PracticaEstrategia> CrearAsync(PracticaEstrategia entidad);
    Task<bool> ActualizarAsync(PracticaEstrategia entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
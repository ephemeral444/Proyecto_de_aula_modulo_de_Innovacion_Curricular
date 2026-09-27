using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioAspectoNormativo
{
    Task<List<AspectoNormativo>> ObtenerTodosAsync();
    Task<AspectoNormativo?> ObtenerPorIdAsync(int id);
    Task<AspectoNormativo> CrearAsync(AspectoNormativo entidad);
    Task<bool> ActualizarAsync(AspectoNormativo entidad);
    Task<bool> BorradoLogicoAsync(int id);
}
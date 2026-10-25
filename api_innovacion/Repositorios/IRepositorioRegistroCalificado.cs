using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioRegistroCalificado
{
	Task<IEnumerable<RegistroCalificado>> ObtenerTodosAsync();
	Task<RegistroCalificado?> ObtenerPorCodigoAsync(string codigo);
	Task<RegistroCalificado> CrearAsync(RegistroCalificado entidad);
    Task<RegistroCalificado?> ActualizarAsync(RegistroCalificado registro);
    Task<bool> EliminarAsync(string codigo);
}
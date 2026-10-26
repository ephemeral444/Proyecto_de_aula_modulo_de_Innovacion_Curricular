using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioActivAcademica
{
    Task<IEnumerable<ActivAcademica>> ObtenerTodosAsync();
    Task<ActivAcademica?> ObtenerPorIdAsync(int id);
    Task<ActivAcademica> CrearAsync(ActivAcademica entidad);
    Task<ActivAcademica?> ActualizarAsync(ActivAcademica entidad);
    Task<bool> EliminarAsync(int id);
}

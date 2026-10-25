using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public interface IRepositorioPasantia
{
    Task<IEnumerable<Pasantia>> ObtenerTodosAsync();
    Task<Pasantia> CrearAsync(Pasantia pasantia);
    Task<Pasantia?> ActualizarAsync(Pasantia entidad);
    Task<bool> EliminarAsync(int id);
}
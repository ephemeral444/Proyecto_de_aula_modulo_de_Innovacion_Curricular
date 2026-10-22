using ApiInnovacion.Modelos;


namespace ApiInnovacion.Repositorios;

public interface IRepositorioFacultad
{
    Task<IEnumerable<Facultad>> ObtenerTodosAsync();
    Task<Facultad?> ObtenerPorIdAsync(int id);
    Task<Facultad> CrearAsync(Facultad facultad);
    Task<Facultad?> ActualizarAsync(Facultad facultad);
    Task<bool> EliminarAsync(int id);
}
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioEnfoque : IServicioEnfoque
{
    private readonly IRepositorioEnfoque _repositorio;

    public ServicioEnfoque(IRepositorioEnfoque repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Enfoque>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Enfoque> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("Enfoque", id);
    }

    public async Task<Enfoque> CrearAsync(EnfoqueCrear peticion)
    {
        var entidad = new Enfoque
        {
            Id = peticion.Id,
            Nombre = peticion.Nombre,
            Descripcion = peticion.Descripcion,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Enfoque> ReemplazarAsync(int id, EnfoqueReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.Nombre = peticion.Nombre;
        existente.Descripcion = peticion.Descripcion;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<Enfoque> ActualizarAsync(int id, EnfoqueActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.Nombre)) existente.Nombre = peticion.Nombre;
        if (!string.IsNullOrWhiteSpace(peticion.Descripcion)) existente.Descripcion = peticion.Descripcion;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.BorradoLogicoAsync(id);
    }
}

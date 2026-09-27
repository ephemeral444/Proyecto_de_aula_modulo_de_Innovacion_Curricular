using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioUniversidad : IServicioUniversidad
{
    private readonly IRepositorioUniversidad _repositorio;

    public ServicioUniversidad(IRepositorioUniversidad repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Universidad>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Universidad> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("Universidad", id);
    }

    public async Task<Universidad> CrearAsync(UniversidadCrear peticion)
    {
        var entidad = new Universidad
        {
            Id = peticion.Id,
            Nombre = peticion.Nombre,
            Tipo = peticion.Tipo,
            Ciudad = peticion.Ciudad,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Universidad> ReemplazarAsync(int id, UniversidadReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.Nombre = peticion.Nombre;
        existente.Tipo = peticion.Tipo;
        existente.Ciudad = peticion.Ciudad;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<Universidad> ActualizarAsync(int id, UniversidadActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.Nombre)) existente.Nombre = peticion.Nombre;
        if (!string.IsNullOrWhiteSpace(peticion.Tipo)) existente.Tipo = peticion.Tipo;
        if (!string.IsNullOrWhiteSpace(peticion.Ciudad)) existente.Ciudad = peticion.Ciudad;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.BorradoLogicoAsync(id);
    }
}

using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioPracticaEstrategia : IServicioPracticaEstrategia
{
    private readonly IRepositorioPracticaEstrategia _repositorio;

    public ServicioPracticaEstrategia(IRepositorioPracticaEstrategia repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<PracticaEstrategia>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<PracticaEstrategia> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("PracticaEstrategia", id);
    }

    public async Task<PracticaEstrategia> CrearAsync(PracticaEstrategiaCrear peticion)
    {
        var entidad = new PracticaEstrategia
        {
            Id = peticion.Id,
            Tipo = peticion.Tipo,
            Nombre = peticion.Nombre,
            Descripcion = peticion.Descripcion,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<PracticaEstrategia> ReemplazarAsync(int id, PracticaEstrategiaReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.Tipo = peticion.Tipo;
        existente.Nombre = peticion.Nombre;
        existente.Descripcion = peticion.Descripcion;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<PracticaEstrategia> ActualizarAsync(int id, PracticaEstrategiaActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.Tipo)) existente.Tipo = peticion.Tipo;
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

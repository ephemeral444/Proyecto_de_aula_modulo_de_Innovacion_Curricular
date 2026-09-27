using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioCarInnovacion : IServicioCarInnovacion
{
    private readonly IRepositorioCarInnovacion _repositorio;

    public ServicioCarInnovacion(IRepositorioCarInnovacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<CarInnovacion>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<CarInnovacion> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("CarInnovacion", id);
    }

    public async Task<CarInnovacion> CrearAsync(CarInnovacionCrear peticion)
    {
        var entidad = new CarInnovacion
        {
            Id = peticion.Id,
            Nombre = peticion.Nombre,
            Descripcion = peticion.Descripcion,
            Tipo = peticion.Tipo,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<CarInnovacion> ReemplazarAsync(int id, CarInnovacionReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.Nombre = peticion.Nombre;
        existente.Descripcion = peticion.Descripcion;
        existente.Tipo = peticion.Tipo;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<CarInnovacion> ActualizarAsync(int id, CarInnovacionActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.Nombre)) existente.Nombre = peticion.Nombre;
        if (!string.IsNullOrWhiteSpace(peticion.Descripcion)) existente.Descripcion = peticion.Descripcion;
        if (!string.IsNullOrWhiteSpace(peticion.Tipo)) existente.Tipo = peticion.Tipo;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.BorradoLogicoAsync(id);
    }
}

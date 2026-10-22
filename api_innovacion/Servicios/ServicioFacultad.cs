using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioFacultad : IServicioFacultad
{
    private readonly IRepositorioFacultad _repositorio;

    public ServicioFacultad(IRepositorioFacultad repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Facultad>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Facultad> ObtenerPorIdAsync(int id)
    {
        var facultad = await _repositorio.ObtenerPorIdAsync(id);
        if (facultad == null)
        {
            throw new NoEncontradoExcepcion($"No se encontró la facultad con el ID {id}.");
        }
        return facultad;
    }

    public async Task<Facultad> CrearAsync(FacultadCrear peticion)
    {
        var entidad = new Facultad
        {
            Nombre = peticion.Nombre,
            UniversidadId = peticion.UniversidadId
        };

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Facultad> ActualizarAsync(int id, FacultadActualizar peticion)
    {
        await ObtenerPorIdAsync(id);

        var entidad = new Facultad
        {
            Id = id,
            Nombre = peticion.Nombre,
            UniversidadId = peticion.UniversidadId
        };

        var resultado = await _repositorio.ActualizarAsync(entidad);
        return resultado ?? throw new NoEncontradoExcepcion($"No se pudo actualizar la facultad con el ID {id}.");
    }

    public async Task EliminarAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.EliminarAsync(id);
    }
}
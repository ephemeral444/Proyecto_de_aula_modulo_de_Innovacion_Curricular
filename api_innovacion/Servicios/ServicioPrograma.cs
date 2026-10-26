using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioPrograma : IServicioPrograma
{
    private readonly IRepositorioPrograma _repositorio;

    public ServicioPrograma(IRepositorioPrograma repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Programa>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Programa> ObtenerPorIdAsync(int id)
    {
        var programa = await _repositorio.ObtenerPorIdAsync(id);
        if (programa == null)
        {
            throw new NoEncontradoExcepcion($"No se encontró el programa con el ID {id}.");
        }
        return programa;
    }

    public async Task<Programa> CrearAsync(ProgramaCrear peticion)
    {
        var entidad = new Programa
        {
            Nombre = peticion.Nombre,
            FacultadId = peticion.FacultadId
        };

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Programa> ActualizarAsync(int id, ProgramaActualizar peticion)
    {
        await ObtenerPorIdAsync(id);

        var entidad = new Programa
        {
            Id = id,
            Nombre = peticion.Nombre,
            FacultadId = peticion.FacultadId
        };

        var resultado = await _repositorio.ActualizarAsync(entidad);
        return resultado ?? throw new NoEncontradoExcepcion($"No se pudo actualizar el programa con el ID {id}.");
    }

    public async Task EliminarAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.EliminarAsync(id);
    }
}
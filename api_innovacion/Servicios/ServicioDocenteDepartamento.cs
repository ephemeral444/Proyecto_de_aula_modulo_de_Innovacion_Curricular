using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Excepciones;

namespace ApiInnovacion.Servicios;

public class ServicioDocenteDepartamento : IServicioDocenteDepartamento
{
    private readonly IRepositorioDocenteDepartamento _repositorio;

    public ServicioDocenteDepartamento(IRepositorioDocenteDepartamento repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<DocenteDepartamento>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<DocenteDepartamento> CrearAsync(DocenteDepartamentoCrear peticion)
    {
        var entidad = new DocenteDepartamento
        {
            DocenteNombre = peticion.DocenteNombre,
            Dedicacion = peticion.Dedicacion,
            Modalidad = peticion.Modalidad,
            FechaInicio = peticion.FechaInicio,
            FechaFin = peticion.FechaFin,
            ProgramaId = peticion.ProgramaId
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<DocenteDepartamento> ActualizarAsync(int id, DocenteDepartamentoActualizar peticion)
    {
        var entidad = new DocenteDepartamento { Id = id, DocenteNombre = peticion.DocenteNombre, Dedicacion = peticion.Dedicacion, Modalidad = peticion.Modalidad, FechaInicio = peticion.FechaInicio, FechaFin = peticion.FechaFin, ProgramaId = peticion.ProgramaId };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró la vinculación docente {id}.");
    }

    public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
}
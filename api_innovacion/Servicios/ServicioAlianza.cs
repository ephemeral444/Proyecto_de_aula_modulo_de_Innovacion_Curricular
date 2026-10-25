using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Excepciones;

namespace ApiInnovacion.Servicios;

public class ServicioAlianza : IServicioAlianza
{
    private readonly IRepositorioAlianza _repositorio;

    public ServicioAlianza(IRepositorioAlianza repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Alianza>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Alianza> CrearAsync(AlianzaCrear peticion)
    {
        var entidad = new Alianza
        {
            AliadoNit = peticion.AliadoNit,
            ProgramaId = peticion.ProgramaId,
            DocenteResponsable = peticion.DocenteResponsable,
            FechaInicio = peticion.FechaInicio,
            FechaFin = peticion.FechaFin
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Alianza> ActualizarAsync(int id, AlianzaActualizar peticion)
    {
        var entidad = new Alianza { Id = id, AliadoNit = peticion.AliadoNit, ProgramaId = peticion.ProgramaId, DocenteResponsable = peticion.DocenteResponsable, FechaInicio = peticion.FechaInicio, FechaFin = peticion.FechaFin };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró la alianza {id}.");
    }

    public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
}
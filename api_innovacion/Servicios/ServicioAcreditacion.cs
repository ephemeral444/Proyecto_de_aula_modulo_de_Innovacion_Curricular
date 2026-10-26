using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioAcreditacion : IServicioAcreditacion
{
    private readonly IRepositorioAcreditacion _repositorio;

    public ServicioAcreditacion(IRepositorioAcreditacion repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Acreditacion>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Acreditacion> CrearAsync(AcreditacionCrear peticion)
    {
        var entidad = new Acreditacion
        {
            Resolucion = peticion.Resolucion,
            Tipo = peticion.Tipo,
            Calificacion = peticion.Calificacion,
            FechaInicio = peticion.FechaInicio,
            FechaFin = peticion.FechaFin,
            ProgramaId = peticion.ProgramaId
        };

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Acreditacion> ActualizarAsync(string resolucion, AcreditacionActualizar peticion)
    {
        var entidad = new Acreditacion
        {
            Resolucion = resolucion,
            Tipo = peticion.Tipo,
            Calificacion = peticion.Calificacion,
            FechaInicio = peticion.FechaInicio,
            FechaFin = peticion.FechaFin,
            ProgramaId = peticion.ProgramaId
        };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró la acreditación {resolucion}.");
    }

    public async Task EliminarAsync(string resolucion)
    {
        var existe = await _repositorio.ObtenerPorResolucionAsync(resolucion);
        if (existe == null)
        {
            throw new NoEncontradoExcepcion($"No se encontró la acreditación {resolucion}.");
        }
        await _repositorio.EliminarAsync(resolucion);
    }
}
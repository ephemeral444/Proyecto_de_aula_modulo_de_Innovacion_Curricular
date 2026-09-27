using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioAreaConocimiento : IServicioAreaConocimiento
{
    private readonly IRepositorioAreaConocimiento _repositorio;

    public ServicioAreaConocimiento(IRepositorioAreaConocimiento repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<AreaConocimiento>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<AreaConocimiento> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("AreaConocimiento", id);
    }

    public async Task<AreaConocimiento> CrearAsync(AreaConocimientoCrear peticion)
    {
        var entidad = new AreaConocimiento
        {
            Id = peticion.Id,
            GranArea = peticion.GranArea,
            Area = peticion.Area,
            Disciplina = peticion.Disciplina,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<AreaConocimiento> ReemplazarAsync(int id, AreaConocimientoReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.GranArea = peticion.GranArea;
        existente.Area = peticion.Area;
        existente.Disciplina = peticion.Disciplina;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<AreaConocimiento> ActualizarAsync(int id, AreaConocimientoActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.GranArea)) existente.GranArea = peticion.GranArea;
        if (!string.IsNullOrWhiteSpace(peticion.Area)) existente.Area = peticion.Area;
        if (!string.IsNullOrWhiteSpace(peticion.Disciplina)) existente.Disciplina = peticion.Disciplina;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.BorradoLogicoAsync(id);
    }
}

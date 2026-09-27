using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioAspectoNormativo : IServicioAspectoNormativo
{
    private readonly IRepositorioAspectoNormativo _repositorio;

    public ServicioAspectoNormativo(IRepositorioAspectoNormativo repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<AspectoNormativo>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<AspectoNormativo> ObtenerPorIdAsync(int id)
    {
        var entidad = await _repositorio.ObtenerPorIdAsync(id);
        return entidad ?? throw new NoEncontradoExcepcion("AspectoNormativo", id);
    }

    public async Task<AspectoNormativo> CrearAsync(AspectoNormativoCrear peticion)
    {
        var entidad = new AspectoNormativo
        {
            Id = peticion.Id,
            Tipo = peticion.Tipo,
            Descripcion = peticion.Descripcion,
            Fuente = peticion.Fuente,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<AspectoNormativo> ReemplazarAsync(int id, AspectoNormativoReemplazo peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        existente.Tipo = peticion.Tipo;
        existente.Descripcion = peticion.Descripcion;
        existente.Fuente = peticion.Fuente;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<AspectoNormativo> ActualizarAsync(int id, AspectoNormativoActualizar peticion)
    {
        var existente = await ObtenerPorIdAsync(id);
        if (!string.IsNullOrWhiteSpace(peticion.Tipo)) existente.Tipo = peticion.Tipo;
        if (!string.IsNullOrWhiteSpace(peticion.Descripcion)) existente.Descripcion = peticion.Descripcion;
        if (!string.IsNullOrWhiteSpace(peticion.Fuente)) existente.Fuente = peticion.Fuente;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(int id)
    {
        await ObtenerPorIdAsync(id);
        await _repositorio.BorradoLogicoAsync(id);
    }
}

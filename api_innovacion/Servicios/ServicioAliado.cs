using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioAliado : IServicioAliado
{
    private readonly IRepositorioAliado _repositorio;

    public ServicioAliado(IRepositorioAliado repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Aliado>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Aliado> ObtenerPorNitAsync(string nit)
    {
        var entidad = await _repositorio.ObtenerPorNitAsync(nit);
        return entidad ?? throw new NoEncontradoExcepcion("Aliado", nit);
    }

    public async Task<Aliado> CrearAsync(AliadoCrear peticion)
    {
        var entidad = new Aliado
        {
            Nit = peticion.Nit,
            RazonSocial = peticion.RazonSocial,
            Contacto = peticion.Contacto,
            Ciudad = peticion.Ciudad,
            Activo = true
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Aliado> ReemplazarAsync(string nit, AliadoReemplazo peticion)
    {
        var existente = await ObtenerPorNitAsync(nit);
        existente.RazonSocial = peticion.RazonSocial;
        existente.Contacto = peticion.Contacto;
        existente.Ciudad = peticion.Ciudad;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task<Aliado> ActualizarAsync(string nit, AliadoActualizar peticion)
    {
        var existente = await ObtenerPorNitAsync(nit);
        if (!string.IsNullOrWhiteSpace(peticion.RazonSocial)) existente.RazonSocial = peticion.RazonSocial;
        if (!string.IsNullOrWhiteSpace(peticion.Contacto)) existente.Contacto = peticion.Contacto;
        if (!string.IsNullOrWhiteSpace(peticion.Ciudad)) existente.Ciudad = peticion.Ciudad;
        await _repositorio.ActualizarAsync(existente);
        return existente;
    }

    public async Task EliminarLogicoAsync(string nit)
    {
        await ObtenerPorNitAsync(nit);
        await _repositorio.BorradoLogicoAsync(nit);
    }
}

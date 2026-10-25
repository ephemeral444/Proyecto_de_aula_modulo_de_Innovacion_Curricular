using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Excepciones;

namespace ApiInnovacion.Servicios;

public class ServicioPremio : IServicioPremio
{
    private readonly IRepositorioPremio _repositorio;

    public ServicioPremio(IRepositorioPremio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Premio>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Premio> CrearAsync(PremioCrear peticion)
    {
        var entidad = new Premio
        {
            Nombre = peticion.Nombre,
            EntidadOtorgante = peticion.EntidadOtorgante,
            FechaOtorgamiento = peticion.FechaOtorgamiento,
            ProgramaId = peticion.ProgramaId
        };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Premio> ActualizarAsync(int id, PremioActualizar peticion)
    {
        var entidad = new Premio { Id = id, Nombre = peticion.Nombre, EntidadOtorgante = peticion.EntidadOtorgante, FechaOtorgamiento = peticion.FechaOtorgamiento, ProgramaId = peticion.ProgramaId };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró el premio {id}.");
    }

    public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
}
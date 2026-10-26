using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Excepciones;

namespace ApiInnovacion.Servicios;

public class ServicioPasantia : IServicioPasantia
{
    private readonly IRepositorioPasantia _repositorio;

    public ServicioPasantia(IRepositorioPasantia repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Pasantia>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();

    public async Task<Pasantia> CrearAsync(PasantiaCrear peticion)
    {
        var entidad = new Pasantia { Nombre = peticion.Nombre, Descripcion = peticion.Descripcion, ProgramaId = peticion.ProgramaId };
        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<Pasantia> ActualizarAsync(int id, PasantiaActualizar peticion)
    {
        var entidad = new Pasantia { Id = id, Nombre = peticion.Nombre, Descripcion = peticion.Descripcion, ProgramaId = peticion.ProgramaId };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró la pasantía {id}.");
    }   

    public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
}
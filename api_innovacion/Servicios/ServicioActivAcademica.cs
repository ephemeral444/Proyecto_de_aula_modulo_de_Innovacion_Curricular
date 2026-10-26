using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioActivAcademica : IServicioActivAcademica
{
    private readonly IRepositorioActivAcademica _repositorio;

    public ServicioActivAcademica(IRepositorioActivAcademica repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<ActivAcademica>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<ActivAcademica> CrearAsync(ActivAcademicaCrear peticion)
    {
        var entidad = new ActivAcademica
        {
            Nombre = peticion.Nombre,
            Creditos = peticion.Creditos,
            Horas = peticion.Horas,
            Idioma = peticion.Idioma,
            EsEspejo = peticion.EsEspejo,
            ProgramaId = peticion.ProgramaId
        };

        return await _repositorio.CrearAsync(entidad);

    }

    public async Task<ActivAcademica> ActualizarAsync(int id, ActivAcademicaActualizar peticion)
    {
        var entidad = new ActivAcademica
        {
            Id = id,
            Nombre = peticion.Nombre,
            Creditos = peticion.Creditos,
            Horas = peticion.Horas,
            Idioma = peticion.Idioma,
            EsEspejo = peticion.EsEspejo,
            ProgramaId = peticion.ProgramaId
        };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró la actividad académica {id}.");
    }

    public async Task EliminarAsync(int id)
    {
        var existe = await _repositorio.ObtenerPorIdAsync(id);
        if (existe == null)
        {
            throw new NoEncontradoExcepcion($"No se encontró la actividad académica con ID {id}.");
        }
        await _repositorio.EliminarAsync(id);
    }
}
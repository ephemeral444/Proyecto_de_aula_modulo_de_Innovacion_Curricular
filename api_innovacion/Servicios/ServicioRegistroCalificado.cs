using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioRegistroCalificado : IServicioRegistroCalificado
{
    private readonly IRepositorioRegistroCalificado _repositorio;

    public ServicioRegistroCalificado(IRepositorioRegistroCalificado repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<RegistroCalificado>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<RegistroCalificado> CrearAsync(RegistroCalificadoCrear peticion)
    {
        var entidad = new RegistroCalificado
        {
            Codigo = peticion.Codigo,
            Creditos = peticion.Creditos,
            Horas = peticion.Horas,
            Metodologia = peticion.Metodologia,
            Duracion = peticion.Duracion,
            Titulacion = peticion.Titulacion,
            ProgramaId = peticion.ProgramaId
        };

        return await _repositorio.CrearAsync(entidad);
    }

    public async Task<RegistroCalificado> ActualizarAsync(string codigo, RegistroCalificadoActualizar peticion)
    {
        var entidad = new RegistroCalificado
        {
            Codigo = codigo,
            Creditos = peticion.Creditos,
            Horas = peticion.Horas,
            Metodologia = peticion.Metodologia,
            Duracion = peticion.Duracion,
            Titulacion = peticion.Titulacion,
            ProgramaId = peticion.ProgramaId
        };
        var res = await _repositorio.ActualizarAsync(entidad);
        return res ?? throw new NoEncontradoExcepcion($"No se encontró el registro calificado {codigo}.");
    }

    public async Task EliminarAsync(string codigo)
    {
        var existe = await _repositorio.ObtenerPorCodigoAsync(codigo);
        if (existe == null)
        {
            throw new NoEncontradoExcepcion($"No se encontró el registro calificado con código {codigo}.");
        }
        await _repositorio.EliminarAsync(codigo);
    }
}
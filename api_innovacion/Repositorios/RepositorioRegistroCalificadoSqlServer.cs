using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioRegistroCalificadoSqlServer : IRepositorioRegistroCalificado
{
    private readonly string _cadenaConexion;

    public RepositorioRegistroCalificadoSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<RegistroCalificado>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<RegistroCalificado>("sp_listar_registro_calificado", commandType: CommandType.StoredProcedure);
    }

    public async Task<RegistroCalificado?> ObtenerPorCodigoAsync(string codigo)
    {
        using var conexion = CrearConexion();
        return await conexion.QueryFirstOrDefaultAsync<RegistroCalificado>(
            "sp_consultar_registro_calificado",
            new { codigo },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<RegistroCalificado> CrearAsync(RegistroCalificado entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var resultado = await conexion.QueryFirstOrDefaultAsync<RegistroCalificado>(
                "sp_crear_registro_calificado",
                new
                {
                    codigo = entidad.Codigo,
                    creditos = entidad.Creditos,
                    horas = entidad.Horas,
                    metodologia = entidad.Metodologia,
                    duracion = entidad.Duracion,
                    titulacion = entidad.Titulacion,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );

            return resultado ?? throw new InvalidOperationException("Error al crear el registro calificado.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new ConflictoExcepcion("Ya existe un registro calificado con ese código.");
        }
    }

    public async Task<RegistroCalificado?> ActualizarAsync(RegistroCalificado entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<RegistroCalificado>(
                "sp_actualizar_registro_calificado",
                new
                {
                    codigo = entidad.Codigo,
                    creditos = entidad.Creditos,
                    horas = entidad.Horas,
                    metodologia = entidad.Metodologia,
                    duracion = entidad.Duracion,
                    titulacion = entidad.Titulacion,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
    }

    public async Task<bool> EliminarAsync(string codigo)
    {
        using var conexion = CrearConexion();
        var filas = await conexion.ExecuteAsync("sp_eliminar_registro_calificado", new { codigo }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
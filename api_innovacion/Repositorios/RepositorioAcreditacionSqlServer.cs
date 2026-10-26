using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioAcreditacionSqlServer : IRepositorioAcreditacion
{
    private readonly string _cadenaConexion;

    public RepositorioAcreditacionSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Acreditacion>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<Acreditacion>("sp_listar_acreditacion", commandType: CommandType.StoredProcedure);
    }

    public async Task<Acreditacion?> ObtenerPorResolucionAsync(string resolucion)
    {
        using var conexion = CrearConexion();
        return await conexion.QueryFirstOrDefaultAsync<Acreditacion>(
            "sp_consultar_acreditacion",
            new { resolucion },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Acreditacion> CrearAsync(Acreditacion entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var resultado = await conexion.QueryFirstOrDefaultAsync<Acreditacion>(
                "sp_crear_acreditacion",
                new
                {
                    resolucion = entidad.Resolucion,
                    tipo = entidad.Tipo,
                    calificacion = entidad.Calificacion,
                    fecha_inicio = entidad.FechaInicio,
                    fecha_fin = entidad.FechaFin,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );

            return resultado ?? throw new InvalidOperationException("Error al crear la acreditación.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new ConflictoExcepcion("Ya existe una acreditación registrada con esa resolución.");
        }
    }

    public async Task<Acreditacion?> ActualizarAsync(Acreditacion entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<Acreditacion>(
                "sp_actualizar_acreditacion",
                new
                {
                    resolucion = entidad.Resolucion,
                    tipo = entidad.Tipo,
                    calificacion = entidad.Calificacion,
                    fecha_inicio = entidad.FechaInicio,
                    fecha_fin = entidad.FechaFin,
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

    public async Task<bool> EliminarAsync(string resolucion)
    {
        using var conexion = CrearConexion();
        var filas = await conexion.ExecuteAsync("sp_eliminar_acreditacion", new { resolucion }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
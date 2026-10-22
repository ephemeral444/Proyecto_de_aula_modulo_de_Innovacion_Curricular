using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;


namespace ApiInnovacion.Repositorios;

public class RepositorioFacultadSqlServer : IRepositorioFacultad
{
    private readonly string _cadenaConexion;

    public RepositorioFacultadSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Facultad>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<Facultad>(
            "sp_listar_facultad",
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Facultad?> ObtenerPorIdAsync(int id)
    {
        using var conexion = CrearConexion();
        return await conexion.QueryFirstOrDefaultAsync<Facultad>(
            "sp_consultar_facultad",
            new { id },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Facultad> CrearAsync(Facultad facultad)
    {
        using var conexion = CrearConexion();
        try
        {
            var resultado = await conexion.QueryFirstOrDefaultAsync<Facultad>(
                "sp_crear_facultad",
                new { nombre = facultad.Nombre, universidad_id = facultad.UniversidadId },
                commandType: CommandType.StoredProcedure
            );

            return resultado ?? throw new InvalidOperationException("Error al crear la facultad.");
        }
        catch (SqlException ex) when (ex.Number == 547) // Violación de clave foránea en SQL Server
        {
            throw new ConflictoExcepcion("La universidad especificada no existe o no está activa.");
        }
    }

    public async Task<Facultad?> ActualizarAsync(Facultad facultad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<Facultad>(
                "sp_actualizar_facultad",
                new { id = facultad.Id, nombre = facultad.Nombre, universidad_id = facultad.UniversidadId },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("La universidad especificada no existe.");
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conexion = CrearConexion();
        var filasAfectadas = await conexion.ExecuteAsync(
            "sp_eliminar_facultad",
            new { id },
            commandType: CommandType.StoredProcedure
        );
        return filasAfectadas > 0;
    }
}
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioPremioSqlServer : IRepositorioPremio
{
    private readonly string _cadenaConexion;

    public RepositorioPremioSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Premio>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<Premio>("sp_listar_premio", commandType: CommandType.StoredProcedure);
    }

    public async Task<Premio> CrearAsync(Premio entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var res = await conexion.QueryFirstOrDefaultAsync<Premio>(
                "sp_crear_premio",
                new
                {
                    nombre = entidad.Nombre,
                    entidad_otorgante = entidad.EntidadOtorgante,
                    fecha_otorgamiento = entidad.FechaOtorgamiento,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );
            return res ?? throw new InvalidOperationException("Error al crear el premio.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
    }

    public async Task<Premio?> ActualizarAsync(Premio entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<Premio>(
                "sp_actualizar_premio",
                new { id = entidad.Id, nombre = entidad.Nombre, entidad_otorgante = entidad.EntidadOtorgante, fecha_otorgamiento = entidad.FechaOtorgamiento, programa_id = entidad.ProgramaId },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conexion = CrearConexion();
        var filas = await conexion.ExecuteAsync("sp_eliminar_premio", new { id }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioAlianzaSqlServer : IRepositorioAlianza
{
    private readonly string _cadenaConexion;

    public RepositorioAlianzaSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Alianza>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<Alianza>("sp_listar_alianza", commandType: CommandType.StoredProcedure);
    }

    public async Task<Alianza> CrearAsync(Alianza entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var res = await conexion.QueryFirstOrDefaultAsync<Alianza>(
                "sp_crear_alianza",
                new
                {
                    aliado_nit = entidad.AliadoNit,
                    programa_id = entidad.ProgramaId,
                    docente_responsable = entidad.DocenteResponsable,
                    fecha_inicio = entidad.FechaInicio,
                    fecha_fin = entidad.FechaFin
                },
                commandType: CommandType.StoredProcedure
            );
            return res ?? throw new InvalidOperationException("Error al registrar la alianza.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El aliado o el programa académico especificado no existe.");
        }
    }

    public async Task<Alianza?> ActualizarAsync(Alianza entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<Alianza>(
                "sp_actualizar_alianza",
                new { id = entidad.Id, aliado_nit = entidad.AliadoNit, programa_id = entidad.ProgramaId, docente_responsable = entidad.DocenteResponsable, fecha_inicio = entidad.FechaInicio, fecha_fin = entidad.FechaFin },
                commandType: CommandType.StoredProcedure
            );
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El aliado o el programa académico no existe.");
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conexion = CrearConexion();
        var filas = await conexion.ExecuteAsync("sp_eliminar_alianza", new { id }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
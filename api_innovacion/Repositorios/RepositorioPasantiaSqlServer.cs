using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioPasantiaSqlServer : IRepositorioPasantia
{
    private readonly string _cadenaConexion;

    public RepositorioPasantiaSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Pasantia>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<Pasantia>("sp_listar_pasantia", commandType: CommandType.StoredProcedure);
    }

    public async Task<Pasantia> CrearAsync(Pasantia entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var res = await conexion.QueryFirstOrDefaultAsync<Pasantia>(
                "sp_crear_pasantia",
                new { nombre = entidad.Nombre, descripcion = entidad.Descripcion, programa_id = entidad.ProgramaId },
                commandType: CommandType.StoredProcedure
            );
            return res ?? throw new InvalidOperationException("Error al crear la pasantía.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico no existe.");
        }
    }

    public async Task<Pasantia?> ActualizarAsync(Pasantia entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<Pasantia>(
                "sp_actualizar_pasantia",
                new { id = entidad.Id, nombre = entidad.Nombre, descripcion = entidad.Descripcion, programa_id = entidad.ProgramaId },
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
        var filas = await conexion.ExecuteAsync("sp_eliminar_pasantia", new { id }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
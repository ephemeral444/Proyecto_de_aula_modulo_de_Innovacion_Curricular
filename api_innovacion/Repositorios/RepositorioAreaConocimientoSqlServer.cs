using System.Data;
using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioAreaConocimientoSqlServer : IRepositorioAreaConocimiento
{
    private readonly string _cadenaConexion;

    public RepositorioAreaConocimientoSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<AreaConocimiento>> ObtenerTodosAsync()
    {
        var lista = new List<AreaConocimiento>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, gran_area, area, disciplina, activo FROM area_conocimiento WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new AreaConocimiento
            {
                Id = lector.GetInt32(0),
                GranArea = lector.GetString(1),
                Area = lector.GetString(2),
                Disciplina = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<AreaConocimiento?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, gran_area, area, disciplina, activo FROM area_conocimiento WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new AreaConocimiento
            {
                Id = lector.GetInt32(0),
                GranArea = lector.GetString(1),
                Area = lector.GetString(2),
                Disciplina = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<AreaConocimiento> CrearAsync(AreaConocimiento entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "INSERT INTO area_conocimiento (id, gran_area, area, disciplina, activo) VALUES (@id, @granArea, @area, @disciplina, 1)", conexion);

        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@granArea", entidad.GranArea);
        comando.Parameters.AddWithValue("@area", entidad.Area);
        comando.Parameters.AddWithValue("@disciplina", entidad.Disciplina);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(AreaConocimiento entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand(
            "UPDATE area_conocimiento SET gran_area = @granArea, area = @area, disciplina = @disciplina WHERE id = @id AND activo = 1", conexion);

        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@granArea", entidad.GranArea);
        comando.Parameters.AddWithValue("@area", entidad.Area);
        comando.Parameters.AddWithValue("@disciplina", entidad.Disciplina);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE area_conocimiento SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioUniversidadSqlServer : IRepositorioUniversidad
{
    private readonly string _cadenaConexion;

    public RepositorioUniversidadSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<Universidad>> ObtenerTodosAsync()
    {
        var lista = new List<Universidad>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, tipo, ciudad, activo FROM universidad WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new Universidad
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Tipo = lector.GetString(2),
                Ciudad = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<Universidad?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, tipo, ciudad, activo FROM universidad WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new Universidad
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Tipo = lector.GetString(2),
                Ciudad = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<Universidad> CrearAsync(Universidad entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO universidad (id, nombre, tipo, ciudad, activo) VALUES (@id, @nombre, @tipo, @ciudad, 1)", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@ciudad", entidad.Ciudad);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(Universidad entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE universidad SET nombre = @nombre, tipo = @tipo, ciudad = @ciudad WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@ciudad", entidad.Ciudad);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE universidad SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
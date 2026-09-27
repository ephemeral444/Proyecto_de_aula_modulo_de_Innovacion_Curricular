using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioEnfoqueSqlServer : IRepositorioEnfoque
{
    private readonly string _cadenaConexion;

    public RepositorioEnfoqueSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<Enfoque>> ObtenerTodosAsync()
    {
        var lista = new List<Enfoque>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, descripcion, activo FROM enfoque WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new Enfoque
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Activo = lector.GetBoolean(3)
            });
        }
        return lista;
    }

    public async Task<Enfoque?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, descripcion, activo FROM enfoque WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new Enfoque
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Activo = lector.GetBoolean(3)
            };
        }
        return null;
    }

    public async Task<Enfoque> CrearAsync(Enfoque entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO enfoque (id, nombre, descripcion, activo) VALUES (@id, @nombre, @descripcion, 1)", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(Enfoque entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE enfoque SET nombre = @nombre, descripcion = @descripcion WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE enfoque SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
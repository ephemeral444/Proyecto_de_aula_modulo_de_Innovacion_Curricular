using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioCarInnovacionSqlServer : IRepositorioCarInnovacion
{
    private readonly string _cadenaConexion;

    public RepositorioCarInnovacionSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<CarInnovacion>> ObtenerTodosAsync()
    {
        var lista = new List<CarInnovacion>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, descripcion, tipo, activo FROM car_innovacion WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new CarInnovacion
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Tipo = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<CarInnovacion?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, nombre, descripcion, tipo, activo FROM car_innovacion WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new CarInnovacion
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Tipo = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<CarInnovacion> CrearAsync(CarInnovacion entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO car_innovacion (id, nombre, descripcion, tipo, activo) VALUES (@id, @nombre, @descripcion, @tipo, 1)", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(CarInnovacion entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE car_innovacion SET nombre = @nombre, descripcion = @descripcion, tipo = @tipo WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE car_innovacion SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioAspectoNormativoSqlServer : IRepositorioAspectoNormativo
{
    private readonly string _cadenaConexion;

    public RepositorioAspectoNormativoSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<AspectoNormativo>> ObtenerTodosAsync()
    {
        var lista = new List<AspectoNormativo>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, tipo, descripcion, fuente, activo FROM aspecto_normativo WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new AspectoNormativo
            {
                Id = lector.GetInt32(0),
                Tipo = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Fuente = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<AspectoNormativo?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, tipo, descripcion, fuente, activo FROM aspecto_normativo WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new AspectoNormativo
            {
                Id = lector.GetInt32(0),
                Tipo = lector.GetString(1),
                Descripcion = lector.GetString(2),
                Fuente = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<AspectoNormativo> CrearAsync(AspectoNormativo entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO aspecto_normativo (id, tipo, descripcion, fuente, activo) VALUES (@id, @tipo, @descripcion, @fuente, 1)", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
        comando.Parameters.AddWithValue("@fuente", entidad.Fuente);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(AspectoNormativo entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE aspecto_normativo SET tipo = @tipo, descripcion = @descripcion, fuente = @fuente WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
        comando.Parameters.AddWithValue("@fuente", entidad.Fuente);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE aspecto_normativo SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
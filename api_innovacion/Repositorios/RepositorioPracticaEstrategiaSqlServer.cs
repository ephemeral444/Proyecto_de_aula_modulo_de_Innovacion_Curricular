using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioPracticaEstrategiaSqlServer : IRepositorioPracticaEstrategia
{
    private readonly string _cadenaConexion;

    public RepositorioPracticaEstrategiaSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<PracticaEstrategia>> ObtenerTodosAsync()
    {
        var lista = new List<PracticaEstrategia>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, tipo, nombre, descripcion, activo FROM practica_estrategia WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new PracticaEstrategia
            {
                Id = lector.GetInt32(0),
                Tipo = lector.GetString(1),
                Nombre = lector.GetString(2),
                Descripcion = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<PracticaEstrategia?> ObtenerPorIdAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT id, tipo, nombre, descripcion, activo FROM practica_estrategia WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new PracticaEstrategia
            {
                Id = lector.GetInt32(0),
                Tipo = lector.GetString(1),
                Nombre = lector.GetString(2),
                Descripcion = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<PracticaEstrategia> CrearAsync(PracticaEstrategia entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO practica_estrategia (id, tipo, nombre, descripcion, activo) VALUES (@id, @tipo, @nombre, @descripcion, 1)", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(PracticaEstrategia entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE practica_estrategia SET tipo = @tipo, nombre = @nombre, descripcion = @descripcion WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", entidad.Id);
        comando.Parameters.AddWithValue("@tipo", entidad.Tipo);
        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@descripcion", entidad.Descripcion);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(int id)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE practica_estrategia SET activo = 0 WHERE id = @id AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@id", id);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
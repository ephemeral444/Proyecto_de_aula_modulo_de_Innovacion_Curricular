using ApiInnovacion.Modelos;
using Microsoft.Data.SqlClient;

namespace ApiInnovacion.Repositorios;

public class RepositorioAliadoSqlServer : IRepositorioAliado
{
    private readonly string _cadenaConexion;

    public RepositorioAliadoSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexi�n 'DefaultConnection' no encontrada.");
    }

    public async Task<List<Aliado>> ObtenerTodosAsync()
    {
        var lista = new List<Aliado>();
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT nit, razon_social, contacto, ciudad, activo FROM aliado WHERE activo = 1", conexion);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            lista.Add(new Aliado
            {
                Nit = lector.GetString(0),
                RazonSocial = lector.GetString(1),
                Contacto = lector.GetString(2),
                Ciudad = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            });
        }
        return lista;
    }

    public async Task<Aliado?> ObtenerPorNitAsync(string nit)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("SELECT nit, razon_social, contacto, ciudad, activo FROM aliado WHERE nit = @nit AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@nit", nit);

        await conexion.OpenAsync();
        using var lector = await comando.ExecuteReaderAsync();
        if (await lector.ReadAsync())
        {
            return new Aliado
            {
                Nit = lector.GetString(0),
                RazonSocial = lector.GetString(1),
                Contacto = lector.GetString(2),
                Ciudad = lector.GetString(3),
                Activo = lector.GetBoolean(4)
            };
        }
        return null;
    }

    public async Task<Aliado> CrearAsync(Aliado entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("INSERT INTO aliado (nit, razon_social, contacto, ciudad, activo) VALUES (@nit, @razonSocial, @contacto, @ciudad, 1)", conexion);
        comando.Parameters.AddWithValue("@nit", entidad.Nit);
        comando.Parameters.AddWithValue("@razonSocial", entidad.RazonSocial);
        comando.Parameters.AddWithValue("@contacto", entidad.Contacto);
        comando.Parameters.AddWithValue("@ciudad", entidad.Ciudad);

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
        return entidad;
    }

    public async Task<bool> ActualizarAsync(Aliado entidad)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE aliado SET razon_social = @razonSocial, contacto = @contacto, ciudad = @ciudad WHERE nit = @nit AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@nit", entidad.Nit);
        comando.Parameters.AddWithValue("@razonSocial", entidad.RazonSocial);
        comando.Parameters.AddWithValue("@contacto", entidad.Contacto);
        comando.Parameters.AddWithValue("@ciudad", entidad.Ciudad);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> BorradoLogicoAsync(string nit)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("UPDATE aliado SET activo = 0 WHERE nit = @nit AND activo = 1", conexion);
        comando.Parameters.AddWithValue("@nit", nit);

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
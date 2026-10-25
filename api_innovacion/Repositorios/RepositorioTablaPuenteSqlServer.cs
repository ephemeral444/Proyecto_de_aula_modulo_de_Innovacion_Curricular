using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;

namespace ApiInnovacion.Repositorios;

public class RepositorioTablaPuenteSqlServer : IRepositorioTablaPuente
{
    private readonly string _cadenaConexion;

    public RepositorioTablaPuenteSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task AsignarAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo)
    {
        using var conexion = CrearConexion();
        try
        {
            var p = new DynamicParameters();
            p.Add(paramPadre, idPadre);
            p.Add(paramHijo, idHijo);

            await conexion.ExecuteAsync(spNombre, p, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new ConflictoExcepcion("Esta relación ya fue asignada previamente.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("Uno de los registros a relacionar no existe.");
        }
    }

    public async Task RetirarAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo)
    {
        using var conexion = CrearConexion();
        var p = new DynamicParameters();
        p.Add(paramPadre, idPadre);
        p.Add(paramHijo, idHijo);

        await conexion.ExecuteAsync(spNombre, p, commandType: CommandType.StoredProcedure);
    }
}
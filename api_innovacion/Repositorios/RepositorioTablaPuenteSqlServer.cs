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
            ?? throw new InvalidOperationException("Cadena no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task AsignarIntIntAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo)
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

    public async Task RetirarIntIntAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo)
    {
        using var conexion = CrearConexion();
        var p = new DynamicParameters();
        p.Add(paramPadre, idPadre);
        p.Add(paramHijo, idHijo);
        await conexion.ExecuteAsync(spNombre, p, commandType: CommandType.StoredProcedure);
    }

    public async Task AsignarIntStringAsync(string spNombre, int idInt, string codigoString)
    {
        using var conexion = CrearConexion();
        try
        {
            await conexion.ExecuteAsync(spNombre, new { enfoque_id = idInt, registro_calificado_codigo = codigoString }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new ConflictoExcepcion("Esta relación ya fue asignada previamente.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El enfoque o el registro calificado especificado no existe.");
        }
    }

    public async Task RetirarIntStringAsync(string spNombre, int idInt, string codigoString)
    {
        using var conexion = CrearConexion();
        await conexion.ExecuteAsync(spNombre, new { enfoque_id = idInt, registro_calificado_codigo = codigoString }, commandType: CommandType.StoredProcedure);
    }

    public async Task RetirarAaRcAsync(
        int activAcademicaId,
        string registroCalificadoCodigo)
    {
        using var conexion = CrearConexion();

        const string sql = @"
        DELETE FROM dbo.aa_rc
        WHERE activ_academica_id = @ActivAcademicaId
          AND registro_calificado_codigo = @RegistroCalificadoCodigo;";

        await conexion.ExecuteAsync(sql, new
        {
            ActivAcademicaId = activAcademicaId,
            RegistroCalificadoCodigo = registroCalificadoCodigo
        }, commandType: CommandType.Text);
    }

    public async Task AsignarAaRcAsync(int activAcademicaId, string registroCalificadoCodigo, string? componente, int? semestre)
    {
        using var conexion = CrearConexion();
        try
        {
            await conexion.ExecuteAsync("sp_asignar_aa_rc", new
            {
                activ_academica_id = activAcademicaId,
                registro_calificado_codigo = registroCalificadoCodigo,
                componente = componente,
                semestre = semestre
            }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new ConflictoExcepcion("Esta actividad académica ya está asignada a este registro calificado.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("La actividad académica o el registro calificado no existe.");
        }
    }
}
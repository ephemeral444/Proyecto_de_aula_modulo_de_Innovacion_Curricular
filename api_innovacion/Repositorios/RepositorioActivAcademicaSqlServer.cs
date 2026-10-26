using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioActivAcademicaSqlServer : IRepositorioActivAcademica
{
    private readonly string _cadenaConexion;

    public RepositorioActivAcademicaSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<ActivAcademica>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<ActivAcademica>("sp_listar_activ_academica", commandType: CommandType.StoredProcedure);
    }

    public async Task<ActivAcademica?> ObtenerPorIdAsync(int id)
    {
        using var conexion = CrearConexion();
        return await conexion.QueryFirstOrDefaultAsync<ActivAcademica>(
            "SELECT aa.id, aa.nombre, aa.creditos, aa.horas, aa.idioma, aa.es_espejo, aa.programa_id, p.nombre AS programa_nombre, aa.activo FROM activ_academica aa INNER JOIN programa p ON aa.programa_id = p.id WHERE aa.id = @id AND aa.activo = 1",
            new { id }
        );
    }

    public async Task<ActivAcademica> CrearAsync(ActivAcademica entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var resultado = await conexion.QueryFirstOrDefaultAsync<ActivAcademica>(
                "sp_crear_activ_academica",
                new
                {
                    nombre = entidad.Nombre,
                    creditos = entidad.Creditos,
                    horas = entidad.Horas,
                    idioma = entidad.Idioma,
                    es_espejo = entidad.EsEspejo,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );

            return resultado ?? throw new InvalidOperationException("Error al crear la actividad académica.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
    }

    public async Task<ActivAcademica?> ActualizarAsync(ActivAcademica entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<ActivAcademica>(
                "sp_actualizar_activ_academica",
                new
                {
                    id = entidad.Id,
                    nombre = entidad.Nombre,
                    creditos = entidad.Creditos,
                    horas = entidad.Horas,
                    idioma = entidad.Idioma,
                    es_espejo = entidad.EsEspejo,
                    programa_id = entidad.ProgramaId
                },
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
        var filas = await conexion.ExecuteAsync("sp_eliminar_activ_academica", new { id }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
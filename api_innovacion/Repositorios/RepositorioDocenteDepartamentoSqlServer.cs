using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Repositorios;

public class RepositorioDocenteDepartamentoSqlServer : IRepositorioDocenteDepartamento
{
    private readonly string _cadenaConexion;

    public RepositorioDocenteDepartamentoSqlServer(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena no configurada.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<DocenteDepartamento>> ObtenerTodosAsync()
    {
        using var conexion = CrearConexion();
        return await conexion.QueryAsync<DocenteDepartamento>("sp_listar_docente_departamento", commandType: CommandType.StoredProcedure);
    }

    public async Task<DocenteDepartamento> CrearAsync(DocenteDepartamento entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            var res = await conexion.QueryFirstOrDefaultAsync<DocenteDepartamento>(
                "sp_crear_docente_departamento",
                new
                {
                    docente_nombre = entidad.DocenteNombre,
                    dedicacion = entidad.Dedicacion,
                    modalidad = entidad.Modalidad,
                    fecha_inicio = entidad.FechaInicio,
                    fecha_fin = entidad.FechaFin,
                    programa_id = entidad.ProgramaId
                },
                commandType: CommandType.StoredProcedure
            );
            return res ?? throw new InvalidOperationException("Error al vincular el docente.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new ConflictoExcepcion("El programa académico especificado no existe.");
        }
    }

    public async Task<DocenteDepartamento?> ActualizarAsync(DocenteDepartamento entidad)
    {
        using var conexion = CrearConexion();
        try
        {
            return await conexion.QueryFirstOrDefaultAsync<DocenteDepartamento>(
                "sp_actualizar_docente_departamento",
                new { id = entidad.Id, docente_nombre = entidad.DocenteNombre, dedicacion = entidad.Dedicacion, modalidad = entidad.Modalidad, fecha_inicio = entidad.FechaInicio, fecha_fin = entidad.FechaFin, programa_id = entidad.ProgramaId },
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
        var filas = await conexion.ExecuteAsync("sp_eliminar_docente_departamento", new { id }, commandType: CommandType.StoredProcedure);
        return filas > 0;
    }
}
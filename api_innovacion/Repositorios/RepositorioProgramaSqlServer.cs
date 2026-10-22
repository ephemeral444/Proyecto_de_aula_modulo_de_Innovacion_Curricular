using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;


namespace ApiInnovacion.Repositorios;

public class RepositorioProgramaSqlServer : IRepositorioPrograma
{
	private readonly string _cadenaConexion;

	public RepositorioProgramaSqlServer(IConfiguration configuracion)
	{
		_cadenaConexion = configuracion.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
	}

	private SqlConnection CrearConexion() => new(_cadenaConexion);

	public async Task<IEnumerable<Programa>> ObtenerTodosAsync()
	{
		using var conexion = CrearConexion();
		return await conexion.QueryAsync<Programa>(
			"sp_listar_programa",
			commandType: CommandType.StoredProcedure
		);
	}

	public async Task<Programa?> ObtenerPorIdAsync(int id)
	{
		using var conexion = CrearConexion();
		return await conexion.QueryFirstOrDefaultAsync<Programa>(
			"sp_consultar_programa",
			new { id },
			commandType: CommandType.StoredProcedure
		);
	}

	public async Task<Programa> CrearAsync(Programa programa)
	{
		using var conexion = CrearConexion();
		try
		{
			var resultado = await conexion.QueryFirstOrDefaultAsync<Programa>(
				"sp_crear_programa",
				new { nombre = programa.Nombre, facultad_id = programa.FacultadId },
				commandType: CommandType.StoredProcedure
			);

			return resultado ?? throw new InvalidOperationException("Error al crear el programa.");
		}
		catch (SqlException ex) when (ex.Number == 547)
		{
			throw new ConflictoExcepcion("La facultad especificada no existe o no está activa.");
		}
	}

	public async Task<Programa?> ActualizarAsync(Programa programa)
	{
		using var conexion = CrearConexion();
		try
		{
			return await conexion.QueryFirstOrDefaultAsync<Programa>(
				"sp_actualizar_programa",
				new { id = programa.Id, nombre = programa.Nombre, facultad_id = programa.FacultadId },
				commandType: CommandType.StoredProcedure
			);
		}
		catch (SqlException ex) when (ex.Number == 547)
		{
			throw new ConflictoExcepcion("La facultad especificada no existe.");
		}
	}

	public async Task<bool> EliminarAsync(int id)
	{
		using var conexion = CrearConexion();
		var filasAfectadas = await conexion.ExecuteAsync(
			"sp_eliminar_programa",
			new { id },
			commandType: CommandType.StoredProcedure
		);
		return filasAfectadas > 0;
	}
}
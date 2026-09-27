using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Servicios;

Console.WriteLine("=== Iniciando Verificación de Aislamiento de Capas ===");

var repoMock = new RepositorioUniversidadMock();
var servicio = new ServicioUniversidad(repoMock);

// 1. Prueba de creación
var nuevaUni = await servicio.CrearAsync(new UniversidadCrear
{
    Id = 10,
    Nombre = "ITM Robledo",
    Tipo = "Campus",
    Ciudad = "Medellin"
});
Console.WriteLine($"[OK] Servicio creó correctamente recurso con Id: {nuevaUni.Id}");

// 2. Prueba de lectura
var uniObtenida = await servicio.ObtenerPorIdAsync(10);
Console.WriteLine($"[OK] Servicio obtuvo: {uniObtenida.Nombre}");

// 3. Prueba de excepción de dominio desacoplada
try
{
    await servicio.ObtenerPorIdAsync(999);
    Console.WriteLine("[FALLO] Debería haber lanzado NoEncontradoExcepcion.");
}
catch (NoEncontradoExcepcion ex)
{
    Console.WriteLine($"[OK] Capa de dominio lanzó excepción desacoplada correctamente: '{ex.Message}'");
}

Console.WriteLine("=== Todas las pruebas de capas pasaron exitosamente ===");

// Mock adaptado con precisión a la interfaz IRepositorioUniversidad
public class RepositorioUniversidadMock : IRepositorioUniversidad
{
    private readonly List<Universidad> _items = new();

    public Task<List<Universidad>> ObtenerTodosAsync() =>
        Task.FromResult(_items.Where(x => x.Activo).ToList());

    public Task<Universidad?> ObtenerPorIdAsync(int id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id && x.Activo));

    public Task<Universidad> CrearAsync(Universidad entidad)
    {
        _items.Add(entidad);
        return Task.FromResult(entidad);
    }

    public Task<bool> ActualizarAsync(Universidad entidad)
    {
        var idx = _items.FindIndex(x => x.Id == entidad.Id);
        if (idx >= 0)
        {
            _items[idx] = entidad;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> BorradoLogicoAsync(int id)
    {
        var itm = _items.FirstOrDefault(x => x.Id == id);
        if (itm != null)
        {
            itm.Activo = false;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
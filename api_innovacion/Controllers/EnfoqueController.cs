using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/enfoques")]
public class EnfoqueController : ControllerBase
{
    private readonly IServicioEnfoque _servicio;

    public EnfoqueController(IServicioEnfoque servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var datos = (await _servicio.ObtenerTodosAsync()).ToList();
        if (datos.Count == 0) return NoContent();
        return Ok(new { recurso = "enfoques", limite = datos.Count, total = datos.Count, datos });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var item = await _servicio.ObtenerPorIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] EnfoqueCrear peticion)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var creado = await _servicio.CrearAsync(peticion);
        return Ok(creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Reemplazar(int id, [FromBody] EnfoqueReemplazo peticion)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var res = await _servicio.ReemplazarAsync(id, peticion);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EnfoqueActualizar peticion)
    {
        var res = await _servicio.ActualizarAsync(id, peticion);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Borrar(int id)
    {
        await _servicio.EliminarLogicoAsync(id);
        return Ok(new { mensaje = "Recurso inactivado correctamente" });
    }
}
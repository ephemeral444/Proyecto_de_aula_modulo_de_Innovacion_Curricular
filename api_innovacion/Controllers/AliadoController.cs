using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/aliados")]
public class AliadoController : ControllerBase
{
    private readonly IServicioAliado _servicio;

    public AliadoController(IServicioAliado servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var datos = (await _servicio.ObtenerTodosAsync()).ToList();
        if (datos.Count == 0) return NoContent();
        return Ok(new { recurso = "aliados", limite = datos.Count, total = datos.Count, datos });
    }

    [HttpGet("{nit}")]
    public async Task<IActionResult> ObtenerPorNit(string nit)
    {
        var item = await _servicio.ObtenerPorNitAsync(nit);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] AliadoCrear peticion)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var creado = await _servicio.CrearAsync(peticion);
        return Ok(creado);
    }

    [HttpPut("{nit}")]
    public async Task<IActionResult> Reemplazar(string nit, [FromBody] AliadoReemplazo peticion)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var res = await _servicio.ReemplazarAsync(nit, peticion);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpPatch("{nit}")]
    public async Task<IActionResult> Actualizar(string nit, [FromBody] AliadoActualizar peticion)
    {
        var res = await _servicio.ActualizarAsync(nit, peticion);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpDelete("{nit}")]
    public async Task<IActionResult> Borrar(string nit)
    {
        await _servicio.EliminarLogicoAsync(nit);
        return Ok(new { mensaje = "Recurso inactivado correctamente" });
    }
}
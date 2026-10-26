using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/acreditacion")]
public class AcreditacionController : ControllerBase
{
    private readonly IServicioAcreditacion _servicio;

    public AcreditacionController(IServicioAcreditacion servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var datos = await _servicio.ObtenerTodosAsync();
        return Ok(datos);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] AcreditacionCrear peticion)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var resultado = await _servicio.CrearAsync(peticion);
            return Ok(resultado);
        }
        catch (ConflictoExcepcion ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{resolucion}")]
    public async Task<IActionResult> Actualizar(string resolucion, [FromBody] AcreditacionActualizar peticion)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var resultado = await _servicio.ActualizarAsync(resolucion, peticion);
            return Ok(resultado);
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ConflictoExcepcion ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{resolucion}")]
    public async Task<IActionResult> Eliminar(string resolucion)
    {
        try
        {
            await _servicio.EliminarAsync(resolucion);
            return NoContent();
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
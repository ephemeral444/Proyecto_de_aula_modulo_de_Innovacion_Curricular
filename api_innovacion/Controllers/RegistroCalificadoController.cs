using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/registro-calificado")]
public class RegistroCalificadoController : ControllerBase
{
    private readonly IServicioRegistroCalificado _servicio;

    public RegistroCalificadoController(IServicioRegistroCalificado servicio)
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
    public async Task<IActionResult> Crear([FromBody] RegistroCalificadoCrear peticion)
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

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] RegistroCalificadoActualizar peticion)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var resultado = await _servicio.ActualizarAsync(codigo, peticion);
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

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Eliminar(string codigo)
    {
        try
        {
            await _servicio.EliminarAsync(codigo);
            return NoContent();
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
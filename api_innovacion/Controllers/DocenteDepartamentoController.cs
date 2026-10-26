using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/docente-departamento")]
public class DocenteDepartamentoController : ControllerBase
{
    private readonly IServicioDocenteDepartamento _servicio;

    public DocenteDepartamentoController(IServicioDocenteDepartamento servicio)
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
    public async Task<IActionResult> Crear([FromBody] DocenteDepartamentoCrear peticion)
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] DocenteDepartamentoActualizar peticion)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var resultado = await _servicio.ActualizarAsync(id, peticion);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _servicio.EliminarAsync(id);
            return NoContent();
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
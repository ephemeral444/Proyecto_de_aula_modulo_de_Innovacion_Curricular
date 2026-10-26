using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/facultad")]
public class FacultadController : ControllerBase
{
    private readonly IServicioFacultad _servicio;

    public FacultadController(IServicioFacultad servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var datos = await _servicio.ObtenerTodosAsync();
        return Ok(datos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        try
        {
            var facultad = await _servicio.ObtenerPorIdAsync(id);
            return Ok(facultad);
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] FacultadCrear peticion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var nuevaFacultad = await _servicio.CrearAsync(peticion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaFacultad.Id }, nuevaFacultad);
        }
        catch (ConflictoExcepcion ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] FacultadActualizar peticion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var facultadActualizada = await _servicio.ActualizarAsync(id, peticion);
            return Ok(facultadActualizada);
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
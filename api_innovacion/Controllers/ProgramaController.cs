using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/programa")]
public class ProgramaController : ControllerBase
{
    private readonly IServicioPrograma _servicio;

    public ProgramaController(IServicioPrograma servicio)
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
            var programa = await _servicio.ObtenerPorIdAsync(id);
            return Ok(programa);
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProgramaCrear peticion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var nuevoPrograma = await _servicio.CrearAsync(peticion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoPrograma.Id }, nuevoPrograma);
        }
        catch (ConflictoExcepcion ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ProgramaActualizar peticion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var programaActualizado = await _servicio.ActualizarAsync(id, peticion);
            return Ok(programaActualizado);
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
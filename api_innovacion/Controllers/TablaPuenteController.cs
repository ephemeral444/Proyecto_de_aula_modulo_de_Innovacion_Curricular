using ApiInnovacion.Excepciones;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Servicios;
using Microsoft.AspNetCore.Mvc;
using ApiInnovacion.Modelos;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/puente")]
public class TablaPuenteController : ControllerBase
{
    private readonly IServicioTablaPuente _servicio;

    public TablaPuenteController(IServicioTablaPuente servicio)
    {
        _servicio = servicio;
    }

    // 1. programa_ac (programa <-> area_conocimiento)
    [HttpPost("programa-ac")]
    public async Task<IActionResult> AsignarProgramaArea([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _servicio.AsignarProgramaAreaAsync(peticion);
            return Ok(new { mensaje = "Área de conocimiento asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-ac")]
    public async Task<IActionResult> RetirarProgramaArea([FromBody] TablaPuentePeticion peticion)
    {
        await _servicio.RetirarProgramaAreaAsync(peticion);
        return NoContent();
    }

    // 2. programa_pe (programa <-> practica_estrategia)
    [HttpPost("programa-pe")]
    public async Task<IActionResult> AsignarProgramaEstrategia([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _servicio.AsignarProgramaEstrategiaAsync(peticion);
            return Ok(new { mensaje = "Práctica/Estrategia asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-pe")]
    public async Task<IActionResult> RetirarProgramaEstrategia([FromBody] TablaPuentePeticion peticion)
    {
        await _servicio.RetirarProgramaEstrategiaAsync(peticion);
        return NoContent();
    }

    // 3. programa_ci (programa <-> car_innovacion)
    [HttpPost("programa-ci")]
    public async Task<IActionResult> AsignarProgramaInnovacion([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _servicio.AsignarProgramaInnovacionAsync(peticion);
            return Ok(new { mensaje = "Característica de innovación asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-ci")]
    public async Task<IActionResult> RetirarProgramaInnovacion([FromBody] TablaPuentePeticion peticion)
    {
        await _servicio.RetirarProgramaInnovacionAsync(peticion);
        return NoContent();
    }

    // 4. an_programa (aspecto_normativo <-> programa)
    [HttpPost("an-programa")]
    public async Task<IActionResult> AsignarAspectoPrograma([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _servicio.AsignarAspectoProgramaAsync(peticion);
            return Ok(new { mensaje = "Aspecto normativo asignado exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("an-programa")]
    public async Task<IActionResult> RetirarAspectoPrograma([FromBody] TablaPuentePeticion peticion)
    {
        await _servicio.RetirarAspectoProgramaAsync(peticion);
        return NoContent();
    }

    // 5. enfoque_rc (enfoque <-> registro_calificado)
    [HttpPost("enfoque-rc")]
    public async Task<IActionResult> AsignarEnfoqueRc([FromBody] TablaPuenteIntStringPeticion peticion)
    {
        try
        {
            await _servicio.AsignarEnfoqueRcAsync(peticion);
            return Ok(new { mensaje = "Enfoque asignado exitosamente al registro calificado." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("enfoque-rc")]
    public async Task<IActionResult> RetirarEnfoqueRc([FromBody] TablaPuenteIntStringPeticion peticion)
    {
        await _servicio.RetirarEnfoqueRcAsync(peticion);
        return NoContent();
    }

    // 6. aa_rc (activ_academica <-> registro_calificado)
    [HttpPost("aa-rc")]
    public async Task<IActionResult> AsignarAaRc([FromBody] TablaPuenteAaRcPeticion peticion)
    {
        try
        {
            await _servicio.AsignarAaRcAsync(peticion);
            return Ok(new { mensaje = "Actividad académica asignada exitosamente al registro calificado." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("aa-rc")]
    public async Task<IActionResult> RetirarAaRc([FromBody] TablaPuenteAaRcPeticion peticion)
    {
        await _servicio.RetirarAaRcAsync(peticion);
        return NoContent();
    }
}
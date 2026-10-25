using ApiInnovacion.Excepciones;
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInnovacion.Controllers;

[ApiController]
[Route("api/puente")]
public class TablaPuenteController : ControllerBase
{
    private readonly IRepositorioTablaPuente _repositorio;

    public TablaPuenteController(IRepositorioTablaPuente repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpPost("programa-ac")]
    public async Task<IActionResult> AsignarProgramaArea([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _repositorio.AsignarAsync("sp_asignar_programa_ac", "@programa_id", "@area_conocimiento_id", peticion.IdPadre, peticion.IdHijo);
            return Ok(new { mensaje = "Área de conocimiento asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-ac")]
    public async Task<IActionResult> RetirarProgramaArea([FromBody] TablaPuentePeticion peticion)
    {
        await _repositorio.RetirarAsync("sp_retirar_programa_ac", "@programa_id", "@area_conocimiento_id", peticion.IdPadre, peticion.IdHijo);
        return NoContent();
    }

    [HttpPost("programa-pe")]
    public async Task<IActionResult> AsignarProgramaEstrategia([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _repositorio.AsignarAsync("sp_asignar_programa_pe", "@programa_id", "@practica_estrategia_id", peticion.IdPadre, peticion.IdHijo);
            return Ok(new { mensaje = "Práctica/Estrategia asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-pe")]
    public async Task<IActionResult> RetirarProgramaEstrategia([FromBody] TablaPuentePeticion peticion)
    {
        await _repositorio.RetirarAsync("sp_retirar_programa_pe", "@programa_id", "@practica_estrategia_id", peticion.IdPadre, peticion.IdHijo);
        return NoContent();
    }

    [HttpPost("programa-ci")]
    public async Task<IActionResult> AsignarProgramaInnovacion([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _repositorio.AsignarAsync("sp_asignar_programa_ci", "@programa_id", "@car_innovacion_id", peticion.IdPadre, peticion.IdHijo);
            return Ok(new { mensaje = "Característica de innovación asignada exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("programa-ci")]
    public async Task<IActionResult> RetirarProgramaInnovacion([FromBody] TablaPuentePeticion peticion)
    {
        await _repositorio.RetirarAsync("sp_retirar_programa_ci", "@programa_id", "@car_innovacion_id", peticion.IdPadre, peticion.IdHijo);
        return NoContent();
    }

    [HttpPost("an-programa")]
    public async Task<IActionResult> AsignarAspectoPrograma([FromBody] TablaPuentePeticion peticion)
    {
        try
        {
            await _repositorio.AsignarAsync("sp_asignar_an_programa", "@aspecto_normativo_id", "@programa_id", peticion.IdPadre, peticion.IdHijo);
            return Ok(new { mensaje = "Aspecto normativo asignado exitosamente al programa." });
        }
        catch (ConflictoExcepcion ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    [HttpDelete("an-programa")]
    public async Task<IActionResult> RetirarAspectoPrograma([FromBody] TablaPuentePeticion peticion)
    {
        await _repositorio.RetirarAsync("sp_retirar_an_programa", "@aspecto_normativo_id", "@programa_id", peticion.IdPadre, peticion.IdHijo);
        return NoContent();
    }
}
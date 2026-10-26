using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;
using ApiInnovacion.Repositorios;

namespace ApiInnovacion.Servicios;

public class ServicioTablaPuente : IServicioTablaPuente
{
    private readonly IRepositorioTablaPuente _repositorio;

    public ServicioTablaPuente(IRepositorioTablaPuente repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task AsignarProgramaAreaAsync(TablaPuentePeticion peticion)
        => await _repositorio.AsignarIntIntAsync("sp_asignar_programa_ac", "@programa_id", "@area_conocimiento_id", peticion.IdPadre, peticion.IdHijo);

    public async Task RetirarProgramaAreaAsync(TablaPuentePeticion peticion)
        => await _repositorio.RetirarIntIntAsync("sp_retirar_programa_ac", "@programa_id", "@area_conocimiento_id", peticion.IdPadre, peticion.IdHijo);

    public async Task AsignarProgramaEstrategiaAsync(TablaPuentePeticion peticion)
        => await _repositorio.AsignarIntIntAsync("sp_asignar_programa_pe", "@programa_id", "@practica_estrategia_id", peticion.IdPadre, peticion.IdHijo);

    public async Task RetirarProgramaEstrategiaAsync(TablaPuentePeticion peticion)
        => await _repositorio.RetirarIntIntAsync("sp_retirar_programa_pe", "@programa_id", "@practica_estrategia_id", peticion.IdPadre, peticion.IdHijo);

    public async Task AsignarProgramaInnovacionAsync(TablaPuentePeticion peticion)
        => await _repositorio.AsignarIntIntAsync("sp_asignar_programa_ci", "@programa_id", "@car_innovacion_id", peticion.IdPadre, peticion.IdHijo);

    public async Task RetirarProgramaInnovacionAsync(TablaPuentePeticion peticion)
        => await _repositorio.RetirarIntIntAsync("sp_retirar_programa_ci", "@programa_id", "@car_innovacion_id", peticion.IdPadre, peticion.IdHijo);

    public async Task AsignarAspectoProgramaAsync(TablaPuentePeticion peticion)
        => await _repositorio.AsignarIntIntAsync("sp_asignar_an_programa", "@aspecto_normativo_id", "@programa_id", peticion.IdPadre, peticion.IdHijo);

    public async Task RetirarAspectoProgramaAsync(TablaPuentePeticion peticion)
        => await _repositorio.RetirarIntIntAsync("sp_retirar_an_programa", "@aspecto_normativo_id", "@programa_id", peticion.IdPadre, peticion.IdHijo);

    public async Task AsignarEnfoqueRcAsync(TablaPuenteIntStringPeticion peticion)
        => await _repositorio.AsignarIntStringAsync("sp_asignar_enfoque_rc", peticion.EnfoqueId, peticion.RegistroCalificadoCodigo);

    public async Task RetirarEnfoqueRcAsync(TablaPuenteIntStringPeticion peticion)
        => await _repositorio.RetirarIntStringAsync("sp_retirar_enfoque_rc", peticion.EnfoqueId, peticion.RegistroCalificadoCodigo);

    public async Task AsignarAaRcAsync(TablaPuenteAaRcPeticion peticion)
        => await _repositorio.AsignarAaRcAsync(peticion.ActivAcademicaId, peticion.RegistroCalificadoCodigo, peticion.Componente, peticion.Semestre);

    public async Task RetirarAaRcAsync(TablaPuenteAaRcPeticion peticion)=> await _repositorio.RetirarAaRcAsync(peticion.ActivAcademicaId,peticion.RegistroCalificadoCodigo);
}
using ApiInnovacion.Modelos;
using ApiInnovacion.Peticiones;


namespace ApiInnovacion.Servicios;

public interface IServicioTablaPuente
{
    Task AsignarProgramaAreaAsync(TablaPuentePeticion peticion);
    Task RetirarProgramaAreaAsync(TablaPuentePeticion peticion);

    Task AsignarProgramaEstrategiaAsync(TablaPuentePeticion peticion);
    Task RetirarProgramaEstrategiaAsync(TablaPuentePeticion peticion);

    Task AsignarProgramaInnovacionAsync(TablaPuentePeticion peticion);
    Task RetirarProgramaInnovacionAsync(TablaPuentePeticion peticion);

    Task AsignarAspectoProgramaAsync(TablaPuentePeticion peticion);
    Task RetirarAspectoProgramaAsync(TablaPuentePeticion peticion);

    Task AsignarEnfoqueRcAsync(TablaPuenteIntStringPeticion peticion);
    Task RetirarEnfoqueRcAsync(TablaPuenteIntStringPeticion peticion);

    Task AsignarAaRcAsync(TablaPuenteAaRcPeticion peticion);
    Task RetirarAaRcAsync(TablaPuenteAaRcPeticion peticion);
}
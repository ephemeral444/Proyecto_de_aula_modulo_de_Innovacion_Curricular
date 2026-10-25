namespace ApiInnovacion.Repositorios;

public interface IRepositorioTablaPuente
{
    Task AsignarIntIntAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo);
    Task RetirarIntIntAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo);
    Task AsignarIntStringAsync(string spNombre, int idInt, string codigoString);
    Task RetirarIntStringAsync(string spNombre, int idInt, string codigoString);
    Task AsignarAaRcAsync(int activAcademicaId, string registroCalificadoCodigo, string? componente, int? semestre);
    Task RetirarAaRcAsync(int activAcademicaId, string registroCalificadoCodigo);
}
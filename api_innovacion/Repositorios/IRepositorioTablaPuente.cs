namespace ApiInnovacion.Repositorios;

public interface IRepositorioTablaPuente
{
    Task AsignarAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo);
    Task RetirarAsync(string spNombre, string paramPadre, string paramHijo, int idPadre, int idHijo);
}
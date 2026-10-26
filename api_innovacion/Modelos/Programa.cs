namespace ApiInnovacion.Modelos;

public class Programa
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int FacultadId { get; set; }
    public string FacultadNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
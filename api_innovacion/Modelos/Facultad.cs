namespace ApiInnovacion.Modelos;

public class Facultad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int UniversidadId { get; set; }
    public string UniversidadNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
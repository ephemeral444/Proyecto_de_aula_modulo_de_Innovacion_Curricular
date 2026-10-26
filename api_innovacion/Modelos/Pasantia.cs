namespace ApiInnovacion.Modelos;

public class Pasantia
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
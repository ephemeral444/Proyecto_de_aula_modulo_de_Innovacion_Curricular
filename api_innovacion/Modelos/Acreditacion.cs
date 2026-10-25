namespace ApiInnovacion.Modelos;

public class Acreditacion
{
    public string Resolucion { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Calificacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
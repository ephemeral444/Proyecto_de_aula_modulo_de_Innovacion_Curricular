namespace ApiInnovacion.Modelos;

public class Alianza
{
    public int Id { get; set; }
    public string AliadoNit { get; set; } = string.Empty;
    public string AliadoNombre { get; set; } = string.Empty;
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public string? DocenteResponsable { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool Activo { get; set; }
}
namespace ApiInnovacion.Modelos;

public class DocenteDepartamento
{
    public int Id { get; set; }
    public string DocenteNombre { get; set; } = string.Empty;
    public string? Dedicacion { get; set; }
    public string? Modalidad { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
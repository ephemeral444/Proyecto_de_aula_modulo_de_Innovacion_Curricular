namespace ApiInnovacion.Modelos;

public class RegistroCalificado
{
    public string Codigo { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int Horas { get; set; }
    public string? Metodologia { get; set; }
    public string? Duracion { get; set; }
    public string? Titulacion { get; set; }
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
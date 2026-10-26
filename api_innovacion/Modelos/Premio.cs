namespace ApiInnovacion.Modelos;

public class Premio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string EntidadOtorgante { get; set; } = string.Empty;
    public DateTime? FechaOtorgamiento { get; set; }
    public int ProgramaId { get; set; }
    public string ProgramaNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

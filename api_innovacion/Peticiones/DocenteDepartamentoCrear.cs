using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class DocenteDepartamentoCrear
{
    [Required]
    public string DocenteNombre { get; set; } = string.Empty;
    public string? Dedicacion { get; set; }
    public string? Modalidad { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }
}
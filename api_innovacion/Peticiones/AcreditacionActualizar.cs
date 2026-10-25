using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class AcreditacionActualizar
{
    [Required]
    public string Tipo { get; set; } = string.Empty;
    public string? Calificacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class AlianzaActualizar
{
    [Required]
    public string AliadoNit { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }

    public string? DocenteResponsable { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}
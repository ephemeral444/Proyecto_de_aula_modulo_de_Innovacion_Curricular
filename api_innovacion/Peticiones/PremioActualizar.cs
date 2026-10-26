using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class PremioActualizar
{
    [Required]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    public string EntidadOtorgante { get; set; } = string.Empty;
    public DateTime? FechaOtorgamiento { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }
}
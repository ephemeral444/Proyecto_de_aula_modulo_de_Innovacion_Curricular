using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class PasantiaActualizar
{
    [Required]
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }
}
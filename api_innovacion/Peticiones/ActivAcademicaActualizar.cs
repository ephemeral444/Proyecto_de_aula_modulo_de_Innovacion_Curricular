using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class ActivAcademicaActualizar
{
    [Required]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    public int Creditos { get; set; }
    [Required]
    public int Horas { get; set; }
    public string Idioma { get; set; } = "Español";
    public bool EsEspejo { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProgramaId { get; set; }
}
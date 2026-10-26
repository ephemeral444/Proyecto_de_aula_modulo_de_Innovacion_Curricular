using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class ProgramaCrear
{
    [Required(ErrorMessage = "El nombre del programa es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La facultad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una facultad válida.")]
    public int FacultadId { get; set; }
}
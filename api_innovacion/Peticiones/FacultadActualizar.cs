using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class FacultadActualizar
{
    [Required(ErrorMessage = "El nombre de la facultad es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La universidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una universidad válida.")]
    public int UniversidadId { get; set; }
}
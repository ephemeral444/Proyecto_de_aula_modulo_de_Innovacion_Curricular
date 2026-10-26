using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class AcreditacionCrear
{
	[Required(ErrorMessage = "La resolución es obligatoria.")]
	public string Resolucion { get; set; } = string.Empty;

	[Required(ErrorMessage = "El tipo de acreditación es obligatorio.")]
	public string Tipo { get; set; } = string.Empty;

	public string? Calificacion { get; set; }
	public DateTime? FechaInicio { get; set; }
	public DateTime? FechaFin { get; set; }

	[Required(ErrorMessage = "El programa es obligatorio.")]
	[Range(1, int.MaxValue, ErrorMessage = "Seleccione un programa válido.")]
	public int ProgramaId { get; set; }
}
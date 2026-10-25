using System.ComponentModel.DataAnnotations;

namespace ApiInnovacion.Peticiones;

public class RegistroCalificadoCrear
{
	[Required(ErrorMessage = "El código es obligatorio.")]
	public string Codigo { get; set; } = string.Empty;

	[Required]
	public int Creditos { get; set; }

	[Required]
	public int Horas { get; set; }

	public string? Metodologia { get; set; }
	public string? Duracion { get; set; }
	public string? Titulacion { get; set; }

	[Required]
	[Range(1, int.MaxValue)]
	public int ProgramaId { get; set; }
}
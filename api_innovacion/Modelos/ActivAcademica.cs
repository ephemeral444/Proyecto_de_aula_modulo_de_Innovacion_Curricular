namespace ApiInnovacion.Modelos;

public class ActivAcademica
{
	public int Id { get; set; }
	public string Nombre { get; set; } = string.Empty;
	public int Creditos { get; set; }
	public int Horas { get; set; }
	public string Idioma { get; set; } = "Español";
	public bool EsEspejo { get; set; }
	public int ProgramaId { get; set; }
	public string ProgramaNombre { get; set; } = string.Empty;
	public bool Activo { get; set; }
}
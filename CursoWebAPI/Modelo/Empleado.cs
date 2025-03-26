namespace CursoWebAPI.Modelo
{
	public class Empleado
	{
		public int Id { get; init; }
		public string Nombre { get; set; }
		public string CodEmpleado { get; set; }
		public string Email { get; set; }
		public int Edad { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
	}
}

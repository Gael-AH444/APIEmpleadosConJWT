using System.ComponentModel.DataAnnotations;

namespace CursoWebAPI.DTO
{
	public class EmpleadoDTO
	{
		[Required(ErrorMessage = "El campo nombre es obligatorio")]
		public string Nombre { get; set; }

		[Required(ErrorMessage = "El campo codigo empleado es obligatorio")]
		[MaxLength(4, ErrorMessage = "Maximo 4 digitos")]
		public string CodEmpleado { get; set; }

		[Required(ErrorMessage = "El campo correo es obligatorio")]
		[EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
		public string Email { get; set; }

		[Required(ErrorMessage = "El campo edad es obligatorio")]
		[Range(16, 90, ErrorMessage = "La edad debe de estar entre 16 y 90")]
		public int Edad { get; set; }
	}
}

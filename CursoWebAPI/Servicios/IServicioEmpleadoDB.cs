using CursoWebAPI.Modelo;

namespace CursoWebAPI.Servicios
{
	public interface IServicioEmpleadoDB
	{
		public Task<IEnumerable<Empleado>> ListarEmpleados();

		public Task<Empleado> ObtenerEmpleadoPorId(string codigo);

		public Task<Empleado> ObtenerEmpleadoBajaPorId(string codigo);

		public Task CrearEmpleado(Empleado empleado);

		public Task EditarEmpleado(Empleado empleado);

		public Task EliminarEmpleado(string codigoEmpleado);

		public Task GuardarImagen(string imgBase64);

		public Task<String> ObtenerImagen(int id);
	}
}

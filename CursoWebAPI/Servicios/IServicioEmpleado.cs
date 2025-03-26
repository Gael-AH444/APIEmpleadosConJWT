using CursoWebAPI.Modelo;

namespace CursoWebAPI.Servicios
{
	public interface IServicioEmpleado
	{
		public IEnumerable<Empleado> ListarEmpleados();

		public Empleado CodigoEmpleado(string codigo);

		public void CrearEmpleado(Empleado empleado);

		public void ModificarEmpleado(Empleado empleado);

		public void BajaEmpleado(string codigoEmpleado);
	}
}

using CursoWebAPI.Modelo;

namespace CursoWebAPI.Servicios
{
	public class ServicioEmpleado : IServicioEmpleado
	{
		private readonly List<Empleado> listaEmpleados = new()
		{
			new Empleado {Id = 1, Nombre="Gael", CodEmpleado="A001", Email="gael@gmail.com", Edad=25, FechaAlta=DateTime.Now, FechaBaja = null},
			new Empleado {Id = 2, Nombre="Juan", CodEmpleado="A002", Email="juan@gmail.com", Edad=35, FechaAlta=DateTime.Now, FechaBaja = null},
			new Empleado {Id = 3, Nombre="Ana", CodEmpleado="A003", Email="ana@gmail.com", Edad=22, FechaAlta=DateTime.Now, FechaBaja = null},
			new Empleado {Id = 4, Nombre="Manuel", CodEmpleado="A004", Email="manuel@gmail.com", Edad=19, FechaAlta=DateTime.Now, FechaBaja = null},
		};

		public IEnumerable<Empleado> ListarEmpleados()
		{
			return listaEmpleados;
		}

		public Empleado CodigoEmpleado(string codigo)
		{
			return listaEmpleados.Where(e => e.CodEmpleado == codigo).SingleOrDefault();
		}

		public void CrearEmpleado(Empleado empleado)
		{
			listaEmpleados.Add(empleado);
		}

		public void ModificarEmpleado(Empleado empleado)
		{
			int posicion = listaEmpleados.FindIndex(e => e.Id == empleado.Id);
			if (posicion != -1)
			{
				listaEmpleados[posicion] = empleado;
			}
		}

		public void BajaEmpleado(string codigoEmpleado)
		{
			int posicion = listaEmpleados.FindIndex(e => e.CodEmpleado == codigoEmpleado);
			if (posicion != -1)
			{
				listaEmpleados.RemoveAt(posicion);
			}
		}
	}
}

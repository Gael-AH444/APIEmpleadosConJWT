using CursoWebAPI.Modelo;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CursoWebAPI.Servicios
{
	public class ServicioEmpleadoDB : IServicioEmpleadoDB
	{
		private readonly string connectionString;

		//Obtener cadena de conexion
		public ServicioEmpleadoDB(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("Conexion");
		}


		public async Task CrearEmpleado(Empleado empleado)
		{
			if (empleado == null)
			{
				throw new ArgumentNullException(nameof(empleado), "El empleado no puede estar vacio.");
			}

			using var connex = new SqlConnection(connectionString);


			await connex.ExecuteAsync(
				"SP_EmpleadoAlta",
				new
				{
					Nombre = empleado.Nombre,
					CodEmpleado = empleado.CodEmpleado,
					Edad = empleado.Edad,
					Email = empleado.Email,
					FechaAlta = empleado.FechaAlta,
				},
				commandType: CommandType.StoredProcedure
			);
		}

		public async Task EditarEmpleado(Empleado empleado)
		{
			if (empleado == null)
			{
				throw new ArgumentNullException(nameof(empleado), "El empleado no puede estar vacio.");
			}

			using var connex = new SqlConnection(connectionString);

			await connex.ExecuteAsync(
					"SP_ModificarEmpleado",
					new
					{
						Nombre = empleado.Nombre,
						CodEmpleado = empleado.CodEmpleado,
						Edad = empleado.Edad,
						Email = empleado.Email
					},
					commandType: CommandType.StoredProcedure
				);
		}

		public async Task EliminarEmpleado(string codigoEmpleado)
		{
			using var connex = new SqlConnection(connectionString);

			await connex.ExecuteAsync(
					"SP_EliminarEmpleado",
					new
					{
						CodEmpleado = codigoEmpleado
					},
					commandType: CommandType.StoredProcedure
			);
		}

		public async Task<IEnumerable<Empleado>> ListarEmpleados()
		{
			using var connex = new SqlConnection(connectionString);

			var empleados = await connex.QueryAsync<Empleado>(
					"SP_EmpleadoObtener",
					commandType: CommandType.StoredProcedure
			);

			return empleados;
		}

		public async Task<Empleado> ObtenerEmpleadoBajaPorId(string codigo)
		{
			if (codigo is null)
			{
				throw new ArgumentNullException(nameof(codigo), "El codigo empleado no puede estar vacio.");
			}

			using var connex = new SqlConnection(connectionString);

			var empleado = await connex.QueryFirstOrDefaultAsync<Empleado>(
					"SP_ObtenerEmpleadoBajaPorId",
					new
					{
						CodEmpleado = codigo
					},
					commandType: CommandType.StoredProcedure
			);

			return empleado;
		}

		public async Task<Empleado> ObtenerEmpleadoPorId(string codigo)
		{
			if (codigo is null)
			{
				throw new ArgumentNullException(nameof(codigo), "El codigo empleado no puede estar vacio.");
			}

			using var connex = new SqlConnection(connectionString);

			var empleado = await connex.QueryFirstOrDefaultAsync<Empleado>(
					"SP_EmpleadoObtener",
					new
					{
						CodEmpleado = codigo
					},
					commandType: CommandType.StoredProcedure
			);

			return empleado;
		}

		public async Task GuardarImagen(string imgBase64)
		{
			if (imgBase64 == null)
			{
				throw new ArgumentNullException(nameof(imgBase64), "La imagen no puede estar vacia.");
			}

			using var connex = new SqlConnection(connectionString);


			await connex.ExecuteAsync(
				"SP_GuardarImagen",
				new
				{
					Imagen = imgBase64
				},
				commandType: CommandType.StoredProcedure
			);
		}

		public async Task<string> ObtenerImagen(int id)
		{
			if (id == 0)
			{
				throw new ArgumentNullException(nameof(id), "El Id no puede estar vacio.");
			}

			using var connex = new SqlConnection(connectionString);

			var img = await connex.QueryFirstOrDefaultAsync<String>(
					"SP_ObtenerImagen",
					new
					{
						Id = id
					},
					commandType: CommandType.StoredProcedure
			);

			return img;
		}
	}
}

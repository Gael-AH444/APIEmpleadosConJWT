using CursoWebAPI.Modelo;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace CursoWebAPI.Servicios
{
	public class ServicioUsuario : IServicioUsuario
	{
		private readonly string connectionString;

		//Obtener cadena de conexion
		public ServicioUsuario(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("Conexion");
		}


		//Obtener usuario
		public async Task<UsuarioAPI> ObtenerUsuario(LoginAPI login)
		{
			if (login is null)
			{
				throw new ArgumentNullException(nameof(login), "Las credenciales no pueden estar vacias");
			}

			using var connex = new SqlConnection(connectionString);

			var usuario = await connex.QueryFirstOrDefaultAsync<UsuarioAPI>(
					"SP_ObtenerUsuario",
					new
					{
						Usuario = login.Usuario,
						Password = login.Password
					},
					commandType: CommandType.StoredProcedure
			);

			return usuario;
		}
	}
}

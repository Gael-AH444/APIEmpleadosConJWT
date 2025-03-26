using CursoWebAPI.Modelo;

namespace CursoWebAPI.Servicios
{
	public interface IServicioUsuario
	{
		public Task<UsuarioAPI> ObtenerUsuario(LoginAPI login);
	}
}

using AutoMapper;
using CursoWebAPI.DTO;
using CursoWebAPI.Modelo;

namespace CursoWebAPI.Utilidades
{
	public static class Utilidades
	{
		/*Con 'this' como parametro de entrada, estamos conviertiendo el metodo, en un metodo de extension de 
		 la clase Empleado*/
		public static EmpleadoDTO ConvertirDTO(this Empleado empleado, IMapper mapper)
		{
			if (empleado is not null)
			{
				//MAPEANDO PROPIEDADES MANUALMENTE
				//return new EmpleadoDTO
				//{
				//	Nombre = empleado.Nombre,
				//	Edad = empleado.Edad,
				//	Email = empleado.Email,
				//	CodEmpleado = empleado.CodEmpleado
				//};

				//MAPEANDO PROPIEDADES CON AUTOMAPPER
				return mapper.Map<EmpleadoDTO>(empleado);
			}

			return null;
		}


		public static UsuarioDTO ConvertirUsuarioADTO(this UsuarioAPI usuario, IMapper mapper)
		{
			if (usuario is not null)
			{
				//return new UsuarioDTO
				//{
				//	Usuario = usuario.Usuario,
				//	Token = usuario.Token
				//};
				return mapper.Map<UsuarioDTO>(usuario);
			}
			return null;
		}
	}
}

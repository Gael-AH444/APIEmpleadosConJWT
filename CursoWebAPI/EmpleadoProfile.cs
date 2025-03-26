using AutoMapper;
using CursoWebAPI.DTO;
using CursoWebAPI.Modelo;

namespace CursoWebAPI
{
	public class EmpleadoProfile : Profile
	{
		public EmpleadoProfile()
		{
			/*Este perfil le dice a AutoMapper que mapee automáticamente las propiedades 
			  con el mismo nombre y tipo entre Empleado y EmpleadoDTO.
			'--> CreateMap<TOrigen, TDestino>() <--'*/
			CreateMap<Empleado, EmpleadoDTO>();
			CreateMap<UsuarioAPI, UsuarioDTO>();

		}
	}
}

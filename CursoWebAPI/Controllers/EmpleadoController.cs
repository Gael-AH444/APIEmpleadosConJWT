using AutoMapper;
using CursoWebAPI.DTO;
using CursoWebAPI.Modelo;
using CursoWebAPI.Servicios;
using CursoWebAPI.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class EmpleadoController : ControllerBase
	{
		private readonly IServicioEmpleadoDB _servicioEmpleado;
		private readonly IMapper _mapper;
		private readonly ILogger<EmpleadoController> _logger;
		private readonly IServicioUsuario _servicioUsuario;

		public EmpleadoController(IServicioEmpleadoDB servicioEmpleado, IMapper mapper,
			ILogger<EmpleadoController> logger, IServicioUsuario servicioUsuario)
		{
			_servicioEmpleado = servicioEmpleado;
			_mapper = mapper;
			_logger = logger;
			_servicioUsuario = servicioUsuario;
		}


		/*Retorna la lista con todos los empleados*/
		[HttpGet("ListaEmpleados")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<EmpleadoDTO>>> ListaEmpleados()
		{
			try
			{
				return Ok((await _servicioEmpleado.ListarEmpleados()).Select(e => e.ConvertirDTO(_mapper)));

			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al crear un nuevo empleado." + ex.Message);
			}

		}


		/*Retorna el empleado por su codigo*/
		[HttpGet("EmpleadoPorCodigo/{codEmpleado}")]
		[Authorize]
		public async Task<ActionResult<EmpleadoDTO>> EmpleadoPorCodigo(string codEmpleado)
		{
			try
			{
				var empleado = (await _servicioEmpleado.ObtenerEmpleadoPorId(codEmpleado)).ConvertirDTO(_mapper);
				if (empleado is null)
				{
					return NotFound("Empleado no encontrado para el codigo solicitado");
				}
				return Ok(empleado);

			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al obtener el empleado." + ex.Message);
			}
		}


		/*Insertar un nuevo empleado a la lista*/
		[HttpPost("CrearNuevoEmpleado")]
		[Authorize]
		public async Task<ActionResult<EmpleadoDTO>> CrearNuevoEmpleado([FromBody] EmpleadoDTO e)
		{
			var verificarCodEmpleado = (await _servicioEmpleado.ObtenerEmpleadoBajaPorId(e.CodEmpleado)).ConvertirDTO(_mapper);
			if (verificarCodEmpleado is not null)
			{
				return StatusCode(400, "El codigo de empleado ingresado ya existe, agregar otro distinto");
			}

			try
			{
				//Mapeamo de EmpleadoDTO a Empleado
				Empleado empleado = new Empleado
				{
					CodEmpleado = e.CodEmpleado,
					Nombre = e.Nombre,
					Email = e.Email,
					Edad = e.Edad,
					FechaAlta = DateTime.Now
				};

				await _servicioEmpleado.CrearEmpleado(empleado);

				return Ok(empleado.ConvertirDTO(_mapper));
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al crearb un nuevo empleado " + ex.Message);
			}
		}


		/*Editando un empleado*/
		[HttpPut("ModificarEmpleado")]
		[Authorize]
		public async Task<ActionResult> ModificarEmpleado([FromBody] EmpleadoDTO empDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var empleadoAux = await _servicioEmpleado.ObtenerEmpleadoPorId(empDTO.CodEmpleado);

				if (empleadoAux is null)
				{
					return NotFound("Empleado no encontrado para el codigo solicitado");
				}

				empleadoAux.Nombre = empDTO.Nombre;
				empleadoAux.Email = empDTO.Email;
				empleadoAux.Edad = empDTO.Edad;

				await _servicioEmpleado.EditarEmpleado(empleadoAux);

				return Ok(empDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al modificar el empleado." + ex.Message);
			}
		}


		/*Eliminando un empleado*/
		[HttpDelete("BorrarEmpleado/{codigoEmpleado}")]
		[Authorize]
		public async Task<ActionResult> BorrarEmpleado(string codigoEmpleado)
		{
			if (string.IsNullOrEmpty(codigoEmpleado))
			{
				return BadRequest("El código de empleado no puede estar vacío.");
			}

			try
			{
				var empleadoAux = await _servicioEmpleado.ObtenerEmpleadoPorId(codigoEmpleado);
				if (empleadoAux is null)
				{
					return NotFound("Empleado no encontrado para el código solicitado.");
				}

				await _servicioEmpleado.EliminarEmpleado(codigoEmpleado);

				return Ok(new { Mensaje = "Registro eliminado correctamente", CodigoEmpleado = codigoEmpleado });
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al eliminar el empleado: " + ex.Message);
			}
		}


		/*Guardar una imagen en la BD*/
		[HttpPost("GuardarImagen")]
		[Authorize]
		public async Task<ActionResult> GuardarImagen([FromBody] string imgBase64)
		{
			try
			{
				await _servicioEmpleado.GuardarImagen(imgBase64);
				return Ok("Imagen guardada correctamente");
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al guardar la imagen: " + ex.Message);
			}
		}

		/*Obtener imagen por ID*/
		[HttpGet("ObtenerImagen/{id}")]
		[Authorize]
		public async Task<ActionResult> ObtenerImagen(int id)
		{
			try
			{
				var imagen = await _servicioEmpleado.ObtenerImagen(id);
				if (imagen is null)
				{
					return NotFound("Imagen no encontrada para el ID solicitado.");
				}
				return Ok(imagen);
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al obtener la imagen: " + ex.Message);
			}
		}
	}
}

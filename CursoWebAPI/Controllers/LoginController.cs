using AutoMapper;
using CursoWebAPI.DTO;
using CursoWebAPI.Modelo;
using CursoWebAPI.Servicios;
using CursoWebAPI.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CursoWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILogger<EmpleadoController> _logger;
		private readonly IServicioUsuario _servicioUsuario;
		private readonly IConfiguration _configuration;

		public LoginController(IMapper mapper, ILogger<EmpleadoController> logger,
			IServicioUsuario servicioUsuario, IConfiguration configuration)
		{
			_mapper = mapper;
			_logger = logger;
			_servicioUsuario = servicioUsuario;
			_configuration = configuration;
		}


		/*Retorna el empleado por su codigo*/
		[HttpPost("Login")]
		[AllowAnonymous]
		public async Task<ActionResult<UsuarioDTO>> Login([FromBody] LoginAPI usuario)
		{
			try
			{
				//Validamos que el usuario exista
				var usuarioAPI = await ValidarUsuario(usuario);

				if (usuarioAPI is null)
				{
					_logger.LogError("ERROR AL INTENTAR INGRESAR CON LAS CREDENCIALES");
					return StatusCode(400, "Credenciales no validas");
				}
				else
				{
					usuarioAPI = GenerarToken(usuarioAPI);
				}

				return Ok(usuarioAPI.ConvertirUsuarioADTO(_mapper));
			}
			catch (Exception ex)
			{
				_logger.LogError("ERROR " + ex.ToString());
				return StatusCode(500, "Ocurrió un error interno al obtener el usuario " + ex.Message);
			}
		}

		private UsuarioAPI GenerarToken(UsuarioAPI usuarioAPI)
		{
			//Cabecera del token
			var _symmetricSecurityKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"])
			);

			var _signingCredentials = new SigningCredentials(
				_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var _header = new JwtHeader(_signingCredentials);

			//Claims del token
			var _claims = new[]
			{
				new Claim("email", usuarioAPI.Email),
				new Claim("usuario", usuarioAPI.Usuario),
				new Claim(JwtRegisteredClaimNames.Email, usuarioAPI.Email)
			};

			//Payload del token
			var _payload = new JwtPayload(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				claims: _claims,
				notBefore: DateTime.Now,
				expires: DateTime.UtcNow.AddMinutes(60)
			);

			//Token
			var _token = new JwtSecurityToken(
				_header,
				_payload
			);

			usuarioAPI.Token = new JwtSecurityTokenHandler().WriteToken(_token);

			return usuarioAPI;
		}

		private async Task<UsuarioAPI> ValidarUsuario(LoginAPI usuario)
		{
			return await _servicioUsuario.ObtenerUsuario(usuario);
		}
	}
}

using CursoWebAPI;
using CursoWebAPI.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "CursoWebAPI", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Autorizacion JWT esquema \r\n\r\n Escribe 'Bearer' [espacio] y escribe el token proporcionado.\r\n\r\nExample: \"Bearer 12345abcdef\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
	opt.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = config["JWT:Issuer"],
		ValidAudience = config["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
		Encoding.UTF8.GetBytes(config["JWT:ClaveSecreta"]))
	};
});

builder.Services.AddAuthorization();

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(EmpleadoProfile));

//Agregando inyeccion de dependencias
builder.Services.AddSingleton<IServicioEmpleadoDB, ServicioEmpleadoDB>();
builder.Services.AddSingleton<IServicioUsuario, ServicioUsuario>();

builder.Host.ConfigureLogging((hostingContext, logging) =>
{
	logging.AddNLog();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

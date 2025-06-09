using EncuestasAPI.Helpers;
using EncuestasAPI.Hubs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.Validators;
using EncuestasAPI.Repositories;
using EncuestasAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
{
	jwtOptions.Audience = builder.Configuration["Jwt:Audience"];
	jwtOptions.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["Jwt:Audience"],
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
		ValidateLifetime = true,
		RoleClaimType = ClaimTypes.Role
	};
});


var Cs = builder.Configuration.GetConnectionString("EncuestasDB");

//Inyeccion de dependencias:
builder.Services.AddDbContext<EncuestasContext>
    (
         x=> x.UseMySql(Cs, ServerVersion.AutoDetect(Cs))
    );

builder.Services.AddAutoMapper(typeof(AutomapperProfile));
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "EncuestasAPI", Version = "v1" });

	var jwtSecurityScheme = new OpenApiSecurityScheme
	{
		Scheme = "bearer",
		BearerFormat = "JWT",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};

	c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ jwtSecurityScheme, Array.Empty<string>() }
	});
});

//repositorios
builder.Services.AddScoped<EstadisticasRepository>();
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
// Registrar JWTService
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped(typeof(UsuarioValidator));
builder.Services.AddScoped(typeof(EncuestaValidator));


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend",
		policy =>
		{
			policy
				.WithOrigins("https://localhost:7058")
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<EstadisticasHub>("/hubs/estadisticas");
app.MapHub<EncuestasHub>("/hubs/encuestas");
app.UseFileServer();
app.Run();

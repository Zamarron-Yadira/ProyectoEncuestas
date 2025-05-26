using EncuestasAPI.Helpers;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var Cs = builder.Configuration.GetConnectionString("EncuestasDB");

//Inyeccion de dependencias:
builder.Services.AddDbContext<EncuestasContext>
    (
         x=> x.UseMySql(Cs, ServerVersion.AutoDetect(Cs))
    );

builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//repositorios
builder.Services.AddScoped<Repository<Encuestas>>();
builder.Services.AddScoped<Repository<Preguntas>>();


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});	

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.UseFileServer();
app.Run();

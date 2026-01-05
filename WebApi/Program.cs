using Aplication.Mappings;
using Aplication.UseCases;
using Domain.Interfaces;
using Infraestructure.Data;
using Infraestructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registro de Repositorios (Capa de Infraestructura)
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IInspeccionRepositorio, InspeccionRepositorio>();

// 3. Registro de Casos de Uso (Capa de Aplicación)
builder.Services.AddScoped<RegistrarUsuario>();
builder.Services.AddScoped<CrearInspeccion>();

// 4. Configuración de AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

// Configuración estándar de la API
builder.Services.AddControllers();

// Swagger para probar tus endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Importante para la gestión de cargos y permisos que definimos
app.UseAuthorization();

app.MapControllers();

app.Run();
using Aplication.Mappings;
using Aplication.UseCases;
using Domain.Interfaces;
using Infraestructure.Data;
using Infraestructure.Repositorios;
using Infraestructure.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IInspeccionRepositorio, InspeccionRepositorio>();

// Use Cases
builder.Services.AddScoped<RegistrarUsuario>();
builder.Services.AddScoped<CrearInspeccion>();
builder.Services.AddScoped<LoginUsuario>();
builder.Services.AddScoped<RefrescarToken>();
builder.Services.AddScoped<GestionarArchivoInspeccion>();

// Servicios
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

builder.Services.AddControllers();

// Configuración de Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com", // TODO: Mover a config
            ValidAudience = "yourdomain.com", // TODO: Mover a config
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("Super_secret_key_that_is_at_least_32_chars_long") // TODO: Mover a config
            )
        };
    });


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

app.UseStaticFiles();

// Middlewares de Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
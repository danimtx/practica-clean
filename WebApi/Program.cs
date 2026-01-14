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
using WebApi.Services;
using WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Aplication.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var MyCorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCorsPolicy,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IInspeccionRepositorio, InspeccionRepositorio>();
builder.Services.AddScoped<ICargoRepositorio, CargoRepositorio>();
builder.Services.AddScoped<INotificacionRepositorio, NotificacionRepositorio>();

// Use Cases
builder.Services.AddScoped<RegistrarUsuario>();
builder.Services.AddScoped<CrearInspeccion>();
builder.Services.AddScoped<LoginUsuario>();
builder.Services.AddScoped<RefrescarToken>();
builder.Services.AddScoped<GestionarArchivoInspeccion>();
builder.Services.AddScoped<GestionarFotoPerfil>();
builder.Services.AddScoped<ActualizarPerfilUsuario>();
builder.Services.AddScoped<GestionarUsuario>();

// Servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IArchivoServicio, ArchivoServicio>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

// MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Aplication.UseCases.RegistrarUsuario).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Infraestructure.Data.AppDbContext).Assembly);
});

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

// Configuración de Autorización y Políticas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminPolicy", policy => policy.RequireClaim("cargo", "SuperAdmin"));

    var permissions = new[] { "inspeccion:crear", "inspeccion:editar", "inspeccion:estado", "inspeccion:archivo:subir", "inspeccion:archivo:borrar", "usuario:gestionar", "cargo:gestionar" };
    foreach (var permission in permissions)
    {
        options.AddPolicy(permission, policy => policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "permisos" && c.Value.Split(',').Contains(permission))
        ));
    }
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

app.UseCors(MyCorsPolicy);

// Middlewares de Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<WebApi.Hubs.NotificationHub>("/notificationHub");

app.Run();
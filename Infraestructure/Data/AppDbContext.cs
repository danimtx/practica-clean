using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Inspeccion> Inspecciones { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Usuario-Cargo
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cargo)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.CargoId);

            // Conversión de Permisos
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Permisos)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            
            // --- SEED DATA ---
            var superAdminCargoId = Guid.Parse("a7e1b52c-1a9d-4da0-9a25-7b3b4f5a7a53");
            var adminCargoId = Guid.Parse("b8f2c9e8-4a1e-4f7b-9c6d-2e3a4b5c6d7e");
            var tecnicoCargoId = Guid.Parse("c9d3e7f6-5b2d-4e8a-9a4c-3f2b1a0c9d8f");
            var invitadoCargoId = Guid.Parse("d0e4f8a5-6c3e-4d9b-8b1d-4a0c9d8f7e6a");

            modelBuilder.Entity<Cargo>().HasData(
                new Cargo { Id = superAdminCargoId, Nombre = "SuperAdmin" },
                new Cargo { Id = adminCargoId, Nombre = "Admin" },
                new Cargo { Id = tecnicoCargoId, Nombre = "Tecnico" },
                new Cargo { Id = invitadoCargoId, Nombre = "Invitado" }
            );

            var allPermissions = new List<string> { "inspeccion:crear", "inspeccion:editar", "inspeccion:estado", "inspeccion:archivo:subir", "inspeccion:archivo:borrar", "usuario:gestionar", "cargo:gestionar" };
            
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = Guid.Parse("f8a8b8e8-8e4f-4b8a-b8e8-f8a8b8e8f8a8"),
                    Nombre = "SuperAdmin",
                    Email = "superadmin@cybercorp.com",
                    PasswordHash = "superadmin123", // TODO: Hashear en un proyecto real
                    EstaActivo = true,
                    CargoId = superAdminCargoId,
                    Permisos = allPermissions,
                    FotoPerfil = "/uploads/profiles/default.png"
                }
            );
        }
    }
}

using Aplication.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class GestionarUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly ICargoRepositorio _cargoRepo;

        public GestionarUsuario(IUsuarioRepositorio usuarioRepo, ICargoRepositorio cargoRepo)
        {
            _usuarioRepo = usuarioRepo;
            _cargoRepo = cargoRepo;
        }

        public async Task Ejecutar(UsuarioGestionDTO dto, ClaimsPrincipal user)
        {
            var adminIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminIdString) || !Guid.TryParse(adminIdString, out var adminId))
            {
                throw new InvalidOperationException("El token de administrador es inválido.");
            }

            var admin = await _usuarioRepo.ObtenerPorIdAsync(adminId);
            if (admin == null || admin.Cargo == null)
            {
                throw new InvalidOperationException("El administrador no tiene un cargo válido o no existe.");
            }

            var usuarioAModificar = await _usuarioRepo.ObtenerPorIdAsync(dto.Id);

            if (usuarioAModificar == null)
            {
                throw new ArgumentException("El usuario que intentas modificar no existe.");
            }
             if (usuarioAModificar.Cargo == null)
            {
                throw new InvalidOperationException("El usuario a modificar no tiene un cargo asignado.");
            }

            // Reglas de negocio
            if (admin.Id == usuarioAModificar.Id)
            {
                throw new InvalidOperationException("No puedes modificarte a ti mismo.");
            }

            if (admin.Cargo.Nombre == "Admin")
            {
                if (usuarioAModificar.Cargo.Nombre == "Admin" || usuarioAModificar.Cargo.Nombre == "SuperAdmin")
                {
                    throw new InvalidOperationException("Un Admin no puede modificar a otro Admin o a un SuperAdmin.");
                }
            }

            // Si pasa las validaciones, procedemos a actualizar
            var nuevoCargo = await _cargoRepo.ObtenerPorIdAsync(dto.CargoId);
            if (nuevoCargo == null)
            {
                throw new ArgumentException("El cargo especificado no existe.");
            }

            usuarioAModificar.CargoId = nuevoCargo.Id;
            usuarioAModificar.EstaActivo = dto.EstaActivo;
            usuarioAModificar.Permisos = dto.Permisos;

            await _usuarioRepo.ActualizarUsuarioAsync(usuarioAModificar);
        }
    }
}

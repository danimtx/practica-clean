using Aplication.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class ActualizarPerfilUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepo;

        public ActualizarPerfilUsuario(IUsuarioRepositorio usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public async Task<Usuario> Ejecutar(Guid userId, PerfilEdicionDTO dto)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(userId);
            if (usuario == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }

            if (!string.IsNullOrEmpty(dto.Nombre))
            {
                usuario.Nombre = dto.Nombre;
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                // TODO: Hash password
                usuario.PasswordHash = dto.Password;
            }

            await _usuarioRepo.ActualizarUsuarioAsync(usuario);

            return usuario;
        }
    }
}

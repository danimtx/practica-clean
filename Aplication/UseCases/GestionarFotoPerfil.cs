using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class GestionarFotoPerfil
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly IArchivoServicio _archivoServicio;
        private const long MaxFileSize = 2 * 1024 * 1024; // 2MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public GestionarFotoPerfil(IUsuarioRepositorio usuarioRepo, IArchivoServicio archivoServicio)
        {
            _usuarioRepo = usuarioRepo;
            _archivoServicio = archivoServicio;
        }

        public async Task<Usuario> Ejecutar(Guid userId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                throw new ArgumentException("El archivo no puede estar vacío.");
            }

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Solo se permiten archivos de imagen (.jpg, .jpeg, .png).");
            }

            if (archivo.Length > MaxFileSize)
            {
                throw new ArgumentException($"El tamaño del archivo no puede exceder los {MaxFileSize / 1024 / 1024}MB.");
            }

            var usuario = await _usuarioRepo.ObtenerPorIdAsync(userId);
            if (usuario == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }
            
            // Eliminar foto anterior si no es la default
            _archivoServicio.EliminarArchivo(usuario.FotoPerfil);

            var nombreArchivo = $"{userId}{extension}";
            using var stream = archivo.OpenReadStream();
            var rutaParaDb = await _archivoServicio.GuardarArchivoAsync(stream, nombreArchivo, archivo.ContentType, "profiles");

            usuario.FotoPerfil = rutaParaDb;
            await _usuarioRepo.ActualizarUsuarioAsync(usuario);

            return usuario;
        }
    }
}

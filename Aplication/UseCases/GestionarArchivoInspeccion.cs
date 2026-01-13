using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class GestionarArchivoInspeccion
    {
        private readonly IInspeccionRepositorio _repositorio;
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "inspecciones");

        public GestionarArchivoInspeccion(IInspeccionRepositorio repositorio)
        {
            _repositorio = repositorio;
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> SubirArchivo(Guid inspeccionId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                throw new ArgumentException("El archivo no puede estar vacío.");
            }

            if (Path.GetExtension(archivo.FileName).ToLowerInvariant() != ".pdf")
            {
                throw new ArgumentException("Solo se permiten archivos PDF.");
            }

            var inspeccion = await _repositorio.ObtenerPorIdAsync(inspeccionId);
            if (inspeccion == null)
            {
                throw new ArgumentException("Inspección no encontrada.");
            }

            // Si ya existe un archivo, lo borramos primero.
            if (!string.IsNullOrEmpty(inspeccion.RutaArchivoPdf))
            {
                var rutaAntigua = Path.Combine(_basePath, Path.GetFileName(inspeccion.RutaArchivoPdf));
                if (File.Exists(rutaAntigua))
                {
                    File.Delete(rutaAntigua);
                }
            }

            var nombreArchivo = $"{inspeccionId}.pdf";
            var rutaCompleta = Path.Combine(_basePath, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var rutaParaDb = $"/uploads/inspecciones/{nombreArchivo}";
            await _repositorio.ActualizarArchivoAsync(inspeccionId, rutaParaDb);

            return rutaParaDb;
        }

        public async Task EliminarArchivo(Guid inspeccionId)
        {
            var inspeccion = await _repositorio.ObtenerPorIdAsync(inspeccionId);
            if (inspeccion == null || string.IsNullOrEmpty(inspeccion.RutaArchivoPdf))
            {
                return;
            }

            var nombreArchivo = Path.GetFileName(inspeccion.RutaArchivoPdf);
            var rutaCompleta = Path.Combine(_basePath, nombreArchivo);

            if (File.Exists(rutaCompleta))
            {
                File.Delete(rutaCompleta);
            }

            await _repositorio.ActualizarArchivoAsync(inspeccionId, null);
        }
    }
}

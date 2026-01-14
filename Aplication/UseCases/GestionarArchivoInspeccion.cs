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
        private readonly IArchivoServicio _archivoServicio;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public GestionarArchivoInspeccion(IInspeccionRepositorio repositorio, IArchivoServicio archivoServicio)
        {
            _repositorio = repositorio;
            _archivoServicio = archivoServicio;
        }

        public async Task<string> SubirArchivo(Guid inspeccionId, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                throw new ArgumentException("El archivo no puede estar vacío.");
            }

            if (archivo.ContentType != "application/pdf")
            {
                throw new ArgumentException("Solo se permiten archivos con formato PDF.");
            }

            if (archivo.Length > MaxFileSize)
            {
                throw new ArgumentException($"El tamaño del archivo no puede exceder los {MaxFileSize / 1024 / 1024}MB.");
            }

            var inspeccion = await _repositorio.ObtenerPorIdAsync(inspeccionId);
            if (inspeccion == null)
            {
                throw new ArgumentException("Inspección no encontrada.");
            }

            var nombreArchivo = $"{inspeccionId}.pdf";
            using var stream = archivo.OpenReadStream();
            var rutaParaDb = await _archivoServicio.GuardarArchivoAsync(stream, nombreArchivo, archivo.ContentType, "inspecciones");

            await _repositorio.ActualizarArchivoAsync(inspeccionId, rutaParaDb);

            return rutaParaDb;
        }

        public async Task EliminarArchivo(Guid inspeccionId)
        {
            var inspeccion = await _repositorio.ObtenerPorIdAsync(inspeccionId);
            if (inspeccion == null)
            {
                 throw new ArgumentException("Inspección no encontrada.");
            }
            
            _archivoServicio.EliminarArchivo(inspeccion.RutaArchivoPdf);

            await _repositorio.ActualizarArchivoAsync(inspeccionId, null);
        }
    }
}

using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Infraestructure.Servicios
{
    public class ArchivoServicio : IArchivoServicio
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUploadPath;

        public ArchivoServicio(IWebHostEnvironment env)
        {
            _env = env;
            _baseUploadPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
            }
        }

        public async Task<string> GuardarArchivoAsync(Stream stream, string nombreArchivo, string contentType, string subfolder)
        {
            var targetFolder = Path.Combine(_baseUploadPath, subfolder);
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var rutaCompleta = Path.Combine(targetFolder, nombreArchivo);

            if (File.Exists(rutaCompleta))
            {
                File.Delete(rutaCompleta);
            }

            using (var fileStream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return "/" + Path.Combine("uploads", subfolder, nombreArchivo).Replace('\\', '/');
        }

        public void EliminarArchivo(string? rutaRelativa)
        {
            if (string.IsNullOrEmpty(rutaRelativa) || rutaRelativa.Contains("default.png")) return;

            var rutaCompleta = Path.Combine(_env.WebRootPath, rutaRelativa.TrimStart('/'));
            if (File.Exists(rutaCompleta))
            {
                File.Delete(rutaCompleta);
            }
        }
    }
}

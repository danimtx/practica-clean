using System.IO;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IArchivoServicio
    {
        Task<string> GuardarArchivoAsync(Stream stream, string nombreArchivo, string contentType, string subfolder);
        void EliminarArchivo(string? rutaRelativa);
    }
}

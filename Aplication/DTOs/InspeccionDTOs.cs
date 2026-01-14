using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CrearInspeccionDTO
    {
        public string NombreCliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string DetallesTecnicos { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public Guid? UsuarioId { get; set; }
    }
    public class InspeccionDetalleDTO
    {
        public Guid Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string DetallesTecnicos { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string NombreResponsable { get; set; } = string.Empty;
        public string? RutaArchivoPdf { get; set; }
    }

    public record InspeccionEstadoDTO(string NuevoEstado);
}

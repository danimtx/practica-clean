using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CrearInspeccionDTO
    {
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public string DetallesTecnicos { get; set; }
        public string Observaciones { get; set; }
        public Guid? UsuarioId { get; set; }
    }
    public class InspeccionDetalleDTO
    {
        public Guid Id { get; set; }
        public string NombreCliente { get; set; }
        public string DetallesTecnicos { get; set; }
        public string Estado { get; set; }
        public string NombreResponsable { get; set; } 
        public string? RutaArchivoPdf { get; set; }
    }

    public record InspeccionEstadoDTO(string NuevoEstado);
}

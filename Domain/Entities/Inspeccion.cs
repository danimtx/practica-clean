using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inspeccion
    {
        public Guid Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string DetallesTecnicos { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;

        public string? RutaArchivoPdf { get; set; }
        public string Estado { get; set; } = string.Empty;

        public Guid? UsuarioId { get; set; }
        public Usuario? Tecnico { get; set; }
    }
}

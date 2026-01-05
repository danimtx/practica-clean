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
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string DetallesTecnicos { get; set; }
        public string Observaciones { get; set; }

        public string? RutaArchivoPdf { get; set; }
        public string Estado { get; set; }

        public Guid? UsuarioId { get; set; }
        public Usuario? Tecnico { get; set; }
    }
}

using System;
using MediatR;

namespace Domain.Events
{
    public class InspeccionAsignadaEvent : INotification
    {
        public Guid IdInspeccion { get; set; }
        public Guid UsuarioIdTecnico { get; set; }
        public string NombreAdmin { get; set; } = string.Empty;
        public string TituloInspeccion { get; set; } = string.Empty;
        public DateTime FechaProgramada { get; set; }
    }
}

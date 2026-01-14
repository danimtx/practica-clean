using System;
using MediatR;

namespace Domain.Events
{
    public class InspeccionAsignadaEvent : INotification
    {
        public Guid IdInspeccion { get; set; }
        public Guid UsuarioIdTecnico { get; set; }
        public string NombreAdmin { get; set; }
        public string TituloInspeccion { get; set; }
        public DateTime FechaProgramada { get; set; }
    }
}

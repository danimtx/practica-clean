using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using MediatR;
using Domain.Interfaces;
using Aplication.Interfaces;
using Domain.Entities;

namespace Infraestructure.EventHandlers
{
    public class InspeccionAsignadaNotificationHandler : INotificationHandler<InspeccionAsignadaEvent>
    {
        private readonly INotificacionRepositorio _notificacionRepo;
        private readonly INotificationService _notificationService;

        public InspeccionAsignadaNotificationHandler(INotificacionRepositorio notificacionRepo, INotificationService notificationService)
        {
            _notificacionRepo = notificacionRepo;
            _notificationService = notificationService;
        }

        public async Task Handle(InspeccionAsignadaEvent notification, CancellationToken cancellationToken)
        {
            // 1. Crear y guardar la notificación en la base de datos
            var mensaje = $"El usuario {notification.NombreAdmin} te asignó la inspección '{notification.TituloInspeccion}' para la fecha {notification.FechaProgramada:dd/MM/yyyy}.";
            
            var nuevaNotificacion = new Notificacion
            {
                UsuarioId = notification.UsuarioIdTecnico,
                Mensaje = mensaje,
                Leido = false,
                Fecha = DateTime.UtcNow
            };

            await _notificacionRepo.CrearAsync(nuevaNotificacion);

            // 2. Enviar la notificación en tiempo real a través del servicio de notificación
            await _notificationService.EnviarNotificacion(nuevaNotificacion);
        }
    }
}

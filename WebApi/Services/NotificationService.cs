using System.Threading.Tasks;
using Aplication.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task EnviarNotificacion(Notificacion notificacion)
        {
            await _hubContext.Clients.User(notificacion.UsuarioId.ToString())
                .SendAsync("RecibirNotificacion", notificacion);
        }
    }
}

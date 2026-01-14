using System.Threading.Tasks;
using Domain.Entities;

namespace Aplication.Interfaces
{
    public interface INotificationService
    {
        Task EnviarNotificacion(Notificacion notificacion);
    }
}

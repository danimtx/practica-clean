using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApi.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // SignalR se encargará de mapear usuarios a conexiones
        // a través del IUserIdProvider personalizado.
        // El hub puede estar vacío si no hay métodos que el cliente llame directamente.
    }
}

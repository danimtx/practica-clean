using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebApi.Hubs
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Intenta obtener el ID de la claim estándar de .NET
            var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Si es nulo, intenta con "sub" (Subject, estándar JWT)
            if (string.IsNullOrEmpty(userId))
            {
                userId = connection.User?.FindFirst("sub")?.Value;
            }

            // Si sigue siendo nulo, intenta con "id" (común en configuraciones personalizadas)
            if (string.IsNullOrEmpty(userId))
            {
                userId = connection.User?.FindFirst("id")?.Value;
            }

            // Si sigue siendo nulo, intenta con "uid"
            if (string.IsNullOrEmpty(userId))
            {
                userId = connection.User?.FindFirst("uid")?.Value;
            }

            return userId;
        }
    }
}

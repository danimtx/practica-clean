using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionRepositorio _notificacionRepo;

        public NotificacionesController(INotificacionRepositorio notificacionRepo)
        {
            _notificacionRepo = notificacionRepo;
        }

        [HttpGet("no-leidas")]
        public async Task<IActionResult> GetNoLeidas()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var notificaciones = await _notificacionRepo.ObtenerNoLeidasAsync(userId);
            return Ok(notificaciones);
        }

        [HttpPatch("marcar-leidas")]
        public async Task<IActionResult> MarcarComoLeidas()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var notificaciones = await _notificacionRepo.ObtenerNoLeidasAsync(userId);
            var ids = notificaciones.Select(n => n.Id);

            if (ids.Any())
            {
                await _notificacionRepo.MarcarComoLeidasAsync(ids);
            }
            
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Guid.Empty;
            }
            return userId;
        }
    }
}

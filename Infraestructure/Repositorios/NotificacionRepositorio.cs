using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositorios
{
    public class NotificacionRepositorio : INotificacionRepositorio
    {
        private readonly AppDbContext _context;

        public NotificacionRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notificacion> CrearAsync(Notificacion notificacion)
        {
            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
            return notificacion;
        }

        public async Task<IEnumerable<Notificacion>> ObtenerNoLeidasAsync(Guid usuarioId)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leido)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }
        
        public async Task MarcarComoLeidaAsync(Guid id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion != null)
            {
                notificacion.Leido = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarcarComoLeidasAsync(IEnumerable<Guid> ids)
        {
            await _context.Notificaciones
                .Where(n => ids.Contains(n.Id))
                .ForEachAsync(n => n.Leido = true);
            
            await _context.SaveChangesAsync();
        }
    }
}

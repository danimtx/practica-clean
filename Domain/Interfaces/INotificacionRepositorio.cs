using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface INotificacionRepositorio
    {
        Task<Notificacion> CrearAsync(Notificacion notificacion);
        Task<IEnumerable<Notificacion>> ObtenerNoLeidasAsync(Guid usuarioId);
        Task MarcarComoLeidasAsync(IEnumerable<Guid> ids);
        Task MarcarComoLeidaAsync(Guid id);
    }
}

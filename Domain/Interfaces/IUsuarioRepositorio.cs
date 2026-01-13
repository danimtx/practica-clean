using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task ActualizarUsuarioAsync(Usuario usuario);
        Task<Usuario?> ObtenerPorIdAsync(Guid id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<Usuario?> ObtenerPorRefreshTokenAsync(string refreshToken);
        Task<IEnumerable<Usuario>> ObtenerTodosAsync(string? cargo = null);
    }
}

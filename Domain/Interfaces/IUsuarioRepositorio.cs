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
        // Para registrar invitados o que el Admin cree cuentas
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);

        // Para que el Admin active cuentas y asigne cargos/permisos
        Task ActualizarUsuarioAsync(Usuario usuario);

        // Para el login y validaciones de Admin
        Task<Usuario?> ObtenerPorIdAsync(Guid id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);

        // Para listar usuarios (útil para el Admin)
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    }
}

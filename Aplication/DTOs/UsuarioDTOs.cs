using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UsuarioRegistroDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid? CargoId { get; set; } // Opcional, para que el admin asigne un cargo
    }

    public class UsuarioGestionDTO
    {
        public Guid Id { get; set; }
        public Guid CargoId { get; set; }
        public bool EstaActivo { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
    }

    public class UsuarioDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
    }

    public class PerfilEdicionDTO
    {
        public string? Nombre { get; set; }
        public string? Password { get; set; }
    }
}

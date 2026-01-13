using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Cargo { get; set; } // "Admin", "Técnico", "Pasante", "Invitado"
        public bool EstaActivo { get; set; }

        public List<string> Permisos { get; set; } = new List<string>();

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

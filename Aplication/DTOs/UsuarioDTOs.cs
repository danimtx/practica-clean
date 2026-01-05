using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class UsuarioRegistroDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Para que el Admin gestione cargos y permisos
    public class UsuarioGestionDTO
    {
        public Guid Id { get; set; }
        public string Cargo { get; set; }
        public bool EstaActivo { get; set; }
        public List<string> Permisos { get; set; }
    }
}

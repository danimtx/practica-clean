using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Notificacion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required]
        public string Mensaje { get; set; } = string.Empty;

        public bool Leido { get; set; } = false;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}

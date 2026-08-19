using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Resena
    {
        public int ResenaId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "El comentario es obligatorio.")]
        [StringLength(
            1000,
            MinimumLength = 5,
            ErrorMessage = "El comentario debe contener entre 5 y 1000 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentario")]
        public string Comentario { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        [Display(Name = "Fecha de reseña")]
        public DateTime FechaResena { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        [Required]
        public int DetalleOrdenId { get; set; }

        [Required(ErrorMessage = "El código del ticket es obligatorio.")]
        [StringLength(
            50,
            ErrorMessage = "El código del ticket no puede superar los 50 caracteres.")]
        [Index("IX_Ticket_CodigoUnico", IsUnique = true)]
        [Display(Name = "Código del ticket")]
        public string CodigoUnico { get; set; }

        [Required]
        [Display(Name = "Fecha de generación")]
        public DateTime FechaGeneracion { get; set; }

        [ForeignKey("DetalleOrdenId")]
        public virtual DetalleOrden DetalleOrden { get; set; }
    }
}
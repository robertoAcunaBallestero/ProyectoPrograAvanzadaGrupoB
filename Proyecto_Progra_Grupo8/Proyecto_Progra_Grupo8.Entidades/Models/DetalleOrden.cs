using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class DetalleOrden
    {
        public DetalleOrden()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int DetalleOrdenId { get; set; }

        [Required]
        public int OrdenId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "La cantidad debe ser mayor que cero.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Range(
            0,
            999999999,
            ErrorMessage = "El precio unitario no puede ser negativo.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("OrdenId")]
        public virtual Orden Orden { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
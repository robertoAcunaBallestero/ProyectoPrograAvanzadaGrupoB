using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Orden
    {
        public Orden()
        {
            Detalles = new HashSet<DetalleOrden>();
        }

        public int OrdenId { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(
            128,
            ErrorMessage = "El identificador del usuario no puede superar los 128 caracteres.")]
        public string UsuarioId { get; set; }

        [Required]
        [Display(Name = "Fecha de compra")]
        public DateTime FechaCompra { get; set; }

        [Range(
            0,
            999999999,
            ErrorMessage = "El total de la orden no puede ser negativo.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        public virtual ICollection<DetalleOrden> Detalles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Orden
    {
        public int OrdenId { get; set; }

        [Required, StringLength(128)]
        public string UsuarioId { get; set; }

        public DateTime FechaCompra { get; set; }

        [Range(0, 999999999)]
        public decimal Total { get; set; }

        public virtual ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();
    }
}

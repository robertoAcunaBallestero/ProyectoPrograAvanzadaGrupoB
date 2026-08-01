using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int DetalleOrdenId { get; set; }

        [Required, StringLength(50)]
        public string CodigoUnico { get; set; }

        public DateTime FechaGeneracion { get; set; }
        public virtual DetalleOrden DetalleOrden { get; set; }
    }
}

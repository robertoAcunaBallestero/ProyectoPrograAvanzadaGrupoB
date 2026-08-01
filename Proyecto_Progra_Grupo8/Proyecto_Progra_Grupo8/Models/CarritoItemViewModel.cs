using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Progra_Grupo8.Models
{
    public class CarritoItemViewModel
    {
        public int EventoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Disponibles { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad debe estar entre 1 y 20 entradas.")]
        public int Cantidad { get; set; }

        public decimal Subtotal => Precio * Cantidad;
    }
}
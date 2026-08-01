using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_Progra_Grupo8.Models
{
    public class AgregarCarritoViewModel
    {
        [Required]
        public int EventoId { get; set; }

        [Range(1, 20, ErrorMessage = "Seleccione entre 1 y 20 entradas.")]
        public int Cantidad { get; set; } = 1;
    }
}
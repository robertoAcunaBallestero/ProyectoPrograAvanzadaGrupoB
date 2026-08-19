using System.ComponentModel.DataAnnotations;

namespace Proyecto_Progra_Grupo8.Models.Api
{
    public class ModerarResenaDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression(
            "Aprobada|Rechazada",
            ErrorMessage = "El estado debe ser 'Aprobada' o 'Rechazada'.")]
        public string Estado { get; set; }
    }
}

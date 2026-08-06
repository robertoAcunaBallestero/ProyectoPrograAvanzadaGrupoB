using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class ImagenEvento
    {
        [Key]
        public int ImagenId { get; set; }

        [Required(ErrorMessage = "El archivo de imagen es obligatorio.")]
        public byte[] Archivo { get; set; }

        [Required(ErrorMessage = "El tipo de contenido es obligatorio.")]
        [StringLength(
            100,
            ErrorMessage = "El tipo de contenido no puede superar los 100 caracteres.")]
        public string TipoContenido { get; set; }

        [Required]
        public int EventoId { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }
    }
}
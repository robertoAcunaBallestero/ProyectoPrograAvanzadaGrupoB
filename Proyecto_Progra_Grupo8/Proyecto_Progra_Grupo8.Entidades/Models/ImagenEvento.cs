using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class ImagenEvento
    {
        [Key]
        public int ImagenId { get; set; }

        [Required]
        public byte[] Archivo { get; set; }

        [StringLength(50)]
        public string TipoContenido { get; set; } 

       
        public int EventoId { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }
    }
}
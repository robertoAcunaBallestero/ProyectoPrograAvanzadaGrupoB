using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class CategoriaEvento
    {
        public CategoriaEvento()
        {
            Eventos = new HashSet<Evento>();
        }

        public int CategoriaEventoId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(
            80,
            MinimumLength = 3,
            ErrorMessage = "El nombre debe contener entre 3 y 80 caracteres.")]
        [Display(Name = "Nombre de la categoría")]
        public string Nombre { get; set; }

        [StringLength(
            250,
            ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
    }
}

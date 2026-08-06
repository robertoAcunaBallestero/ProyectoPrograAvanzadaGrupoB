using Proyecto_Progra_Grupo8.Entidades.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Proyecto_Progra_Grupo8.ViewModels
{
    public class EventoViewModel
    {
        public Evento Evento { get; set; }

        [Display(Name = "Imágenes del evento")]
        public IEnumerable<HttpPostedFileBase> ArchivosImagenes { get; set; }
    }
}
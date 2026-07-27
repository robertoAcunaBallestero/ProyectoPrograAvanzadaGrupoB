using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Proyecto_Progra_Grupo8.Entidades.Models;

namespace Proyecto_Progra_Grupo8.ViewModels
{
    public class EventoViewModel
    {
        // Aquí puedes replicar las propiedades de Evento.cs (CodigoEvento, Nombre, Descripcion, etc.)
        public Evento Evento { get; set; }

        [Display(Name = "Imágenes del Evento (Mínimo 3 recomendadas)")]
        // Esta propiedad es la que captura los archivos desde el formulario HTML (<input type="file" multiple />)
        public IEnumerable<HttpPostedFileBase> ArchivosImagenes { get; set; }
    }
}
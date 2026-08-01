using Proyecto_Progra_Grupo8.Negocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    public class CarteleraController : Controller
    {
        private readonly ICarteleraService _service = new CarteleraService();

        [AllowAnonymous]
        public ActionResult Index() => View(_service.ObtenerCartelera());

        [AllowAnonymous]
        public ActionResult Detalle(int id)
        {
            var evento = _service.ObtenerDetalle(id);
            if (evento == null) return HttpNotFound();
            return View(evento);
        }

        [AllowAnonymous]
        public ActionResult Imagen(int id)
        {
            var imagen = _service.ObtenerImagenPrincipal(id);
            if (imagen == null || imagen.Archivo == null) return HttpNotFound();
            return File(imagen.Archivo, string.IsNullOrWhiteSpace(imagen.TipoContenido) ? "image/jpeg" : imagen.TipoContenido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _service.Dispose();
            base.Dispose(disposing);
        }
    }
}
using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class OrdenesController : Controller
    {
        private readonly ICompraService _service;

        public OrdenesController(ICompraService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            _service = service;
        }

        public ActionResult Index()
        {
            var ordenes =
                _service.Historial(
                    User.Identity.GetUserId());

            return View(ordenes);
        }

        public ActionResult Detalle(int id)
        {
            var orden =
                _service.DetalleOrden(
                    id,
                    User.Identity.GetUserId());

            if (orden == null)
            {
                return HttpNotFound();
            }

            return View(orden);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
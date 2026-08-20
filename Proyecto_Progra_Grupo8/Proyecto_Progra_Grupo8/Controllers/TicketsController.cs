using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class TicketsController : Controller
    {
        private readonly ICompraService _service;

        public TicketsController(ICompraService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            _service = service;
        }

        public ActionResult Index()
        {
            var ordenes = _service.Historial(
                User.Identity.GetUserId());

            return View(ordenes);
        }

        //public ActionResult Detalle(int id)
        //{
        //    var ordenes = _service.Historial(
        //        User.Identity.GetUserId());

        //    foreach (var orden in ordenes)
        //    {
        //        foreach (var detalle in orden.Detalles)
        //        {
        //            var ticket = detalle.Tickets
        //                .FirstOrDefault(t => t.TicketId == id);

        //            if (ticket != null)
        //            {
        //                return View(ticket);
        //            }
        //        }
        //    }

        //    return HttpNotFound();
        //} 

        public ActionResult Detalle(int id, int? ordenId)
        {
            var ordenes = _service.Historial(
                User.Identity.GetUserId());

            foreach (var orden in ordenes)
            {
                if (ordenId.HasValue && orden.OrdenId != ordenId.Value)
                {
                    continue;
                }

                foreach (var detalle in orden.Detalles)
                {
                    var ticket = detalle.Tickets
                        .FirstOrDefault(t => t.TicketId == id);

                    if (ticket != null)
                    {
                        //ViewBag.OrdenId = orden.OrdenId; 
                        if (ordenId.HasValue)
                        {
                            ViewBag.OrdenId = orden.OrdenId;
                        }
                        return View(ticket);
                    }
                }
            }

            return HttpNotFound();
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
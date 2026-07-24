using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System.Net;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{

    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;


        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }


        public ActionResult Index()
        {
            ViewBag.TotalEventosActivos =
                _eventoService.ContarActivos();

            var eventos =
                _eventoService.ObtenerTodos();

            return View(eventos);
        }


        public ActionResult Cartelera()
        {
            var eventos =
                _eventoService.ObtenerCatalogo();

            return View(eventos);
        }


        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            Evento evento =
                _eventoService.ObtenerDetalle(id.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            return View(evento);
        }


        public ActionResult Create()
        {
            return View(new Evento());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return View(evento);
            }

            ResultadoOperacion resultado =
                _eventoService.Crear(evento);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                return View(evento);
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            Evento evento =
                _eventoService.ObtenerParaEdicion(
                    id.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            return View(evento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return View(evento);
            }

            ResultadoOperacion resultado =
                _eventoService.Actualizar(evento);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                return View(evento);
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction("Index");
        }


        public ActionResult Desactivar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            Evento evento =
                _eventoService.ObtenerParaEdicion(
                    id.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            return View(evento);
        }


        [HttpPost]
        [ActionName("Desactivar")]
        [ValidateAntiForgeryToken]
        public ActionResult DesactivarConfirmado(int id)
        {
            ResultadoOperacion resultado =
                _eventoService.Desactivar(id);

            if (!resultado.Exito)
            {
                TempData["MensajeError"] =
                    resultado.Mensaje;

                return RedirectToAction("Index");
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction("Index");
        }


        [HttpGet]
        public JsonResult CodigoDisponible(
            string codigoEvento,
            int eventoId = 0)
        {
            bool disponible =
                _eventoService.CodigoDisponible(
                    codigoEvento,
                    eventoId);

            return Json(
                disponible,
                JsonRequestBehavior.AllowGet);
        }
    }
}
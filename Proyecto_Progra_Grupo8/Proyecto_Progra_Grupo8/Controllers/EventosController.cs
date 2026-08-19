using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;
using Proyecto_Progra_Grupo8.ViewModels;
using System.Net;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize(Roles = "Administrador")]
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

        [AllowAnonymous]
        public ActionResult Cartelera()
        {
            var eventos =
                _eventoService.ObtenerCatalogo();

            return View(eventos);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public ActionResult Imagen(int id)
        {
            ImagenEvento imagen =
                _eventoService.ObtenerImagen(id);

            if (imagen == null ||
                imagen.Archivo == null)
            {
                return HttpNotFound();
            }

            return File(
                imagen.Archivo,
                imagen.TipoContenido);
        }

        public ActionResult Create()
        {
            return View(
                new EventoViewModel
                {
                    Evento = new Evento()
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            EventoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            ResultadoOperacion resultado =
                _eventoService.Crear(
                    viewModel.Evento,
                    viewModel.ArchivosImagenes);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                return View(viewModel);
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

            var viewModel =
                new EventoViewModel
                {
                    Evento = evento
                };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            EventoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            ResultadoOperacion resultado =
                _eventoService.Actualizar(
                    viewModel.Evento,
                    viewModel.ArchivosImagenes);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                return View(viewModel);
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

        // La verificación de código disponible se hace ahora vía AJAX
        // contra GET api/eventos/codigo-disponible (EventosApiController).
    }
}
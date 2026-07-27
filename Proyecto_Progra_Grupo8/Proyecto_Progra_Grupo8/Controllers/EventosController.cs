using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;
using Proyecto_Progra_Grupo8.ViewModels; 
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    // Solo los administradores tienen acceso a gestionar el CRUD de eventos
    [Authorize(Roles = "Administrador")]
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // Permitimos que todos vean el index si lo deseas, o puedes dejarlo restringido
        public ActionResult Index()
        {
            ViewBag.TotalEventosActivos = _eventoService.ContarActivos();
            var eventos = _eventoService.ObtenerTodos();
            return View(eventos);
        }

        [AllowAnonymous] // Permitimos a cualquiera (incluso no logueados o Asociados) ver la cartelera
        public ActionResult Cartelera()
        {
            var eventos = _eventoService.ObtenerCatalogo();
            return View(eventos);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Evento evento = _eventoService.ObtenerDetalle(id.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            return View(evento);
        }

        // --- INICIO DE CAMBIOS PARA IMÁGENES (PUNTO 4) ---

        public ActionResult Create()
        {
            // Retornamos el ViewModel vacío en lugar de la entidad pura
            return View(new EventoViewModel { Evento = new Evento() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Pasamos tanto el Evento como la colección de archivos al servicio
            ResultadoOperacion resultado = _eventoService.Crear(viewModel.Evento, viewModel.ArchivosImagenes);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(viewModel);
            }

            TempData["MensajeExito"] = resultado.Mensaje;
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Evento evento = _eventoService.ObtenerParaEdicion(id.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            // Envolvemos el evento recuperado dentro del ViewModel
            var viewModel = new EventoViewModel { Evento = evento };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Pasamos las nuevas imágenes al actualizar (si el usuario decidió subir nuevas)
            ResultadoOperacion resultado = _eventoService.Actualizar(viewModel.Evento, viewModel.ArchivosImagenes);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(viewModel);
            }

            TempData["MensajeExito"] = resultado.Mensaje;
            return RedirectToAction("Index");
        }

        // --- FIN DE CAMBIOS PARA IMÁGENES ---

        public ActionResult Desactivar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Evento evento = _eventoService.ObtenerParaEdicion(id.Value);

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
            ResultadoOperacion resultado = _eventoService.Desactivar(id);

            if (!resultado.Exito)
            {
                TempData["MensajeError"] = resultado.Mensaje;
                return RedirectToAction("Index");
            }

            TempData["MensajeExito"] = resultado.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult CodigoDisponible(string codigoEvento, int eventoId = 0)
        {
            bool disponible = _eventoService.CodigoDisponible(codigoEvento, eventoId);
            return Json(disponible, JsonRequestBehavior.AllowGet);
        }
    }
}
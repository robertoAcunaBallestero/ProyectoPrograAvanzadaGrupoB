using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System;
using System.Net;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize]
    public class ResenasController : Controller
    {
        private readonly IResenaService _resenaService;
        private readonly IEventoService _eventoService;

        public ResenasController(
            IResenaService resenaService,
            IEventoService eventoService)
        {
            if (resenaService == null)
            {
                throw new ArgumentNullException(
                    nameof(resenaService));
            }

            if (eventoService == null)
            {
                throw new ArgumentNullException(
                    nameof(eventoService));
            }

            _resenaService = resenaService;
            _eventoService = eventoService;
        }

        [Authorize(Roles = "Asociado")]
        public ActionResult MisResenas()
        {
            string usuarioId =
                User.Identity.GetUserId();

            var resenas =
                _resenaService.ObtenerPorUsuario(
                    usuarioId);

            return View(resenas);
        }

        [Authorize(Roles = "Asociado")]
        public ActionResult Crear(int? eventoId)
        {
            if (!eventoId.HasValue)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            Evento evento =
                _eventoService.ObtenerDetalle(
                    eventoId.Value);

            if (evento == null)
            {
                return HttpNotFound();
            }

            ViewBag.NombreEvento =
                evento.Nombre;

            var resena = new Resena
            {
                EventoId = evento.EventoId
            };

            return View(resena);
        }

        [HttpPost]
        [Authorize(Roles = "Asociado")]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(
            int eventoId,
            string comentario)
        {
            ResultadoOperacion resultado =
                _resenaService.Crear(
                    eventoId,
                    User.Identity.GetUserId(),
                    comentario);

            if (!resultado.Exito)
            {
                Evento evento =
                    _eventoService.ObtenerDetalle(
                        eventoId);

                if (evento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.NombreEvento =
                    evento.Nombre;

                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                var resena = new Resena
                {
                    EventoId = eventoId,
                    Comentario = comentario
                };

                return View(resena);
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction(
                "MisResenas");
        }

        // Aprobar/Rechazar se hacen ahora vía AJAX contra
        // PUT api/resenas/{id} (ResenasApiController), sin recargar la página.
        [Authorize(Roles = "Administrador")]
        public ActionResult Moderar()
        {
            var resenas =
                _resenaService.ObtenerPendientes();

            return View(resenas);
        }

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _resenaService.Dispose();
                _eventoService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
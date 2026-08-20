using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;
using Proyecto_Progra_Grupo8.ViewModels;
using System;
using System.Net;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    // Solo accesible para usuarios en el rol Administrador.
    [Authorize(Roles = "Administrador")]
    public class AdministracionController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        private readonly string[] _roles =
        {
            "Administrador",
            "Asociado"
        };

        public AdministracionController(
            IUsuarioService usuarioService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(
                    nameof(usuarioService));
            }

            _usuarioService = usuarioService;
        }

        // GET: Administracion
        public ActionResult Index()
        {
            var usuarios =
                _usuarioService.ObtenerTodos();

            return View(usuarios);
        }

        // GET: Administracion/Details/id
        public ActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            ApplicationUser usuario =
                _usuarioService.ObtenerPorId(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.Rol =
                _usuarioService.ObtenerRol(id);

            return View(usuario);
        }

        // GET: Administracion/Create
        public ActionResult Create()
        {
            PrepararRoles("Asociado");

            var model = new UsuarioViewModel
            {
                Rol = "Asociado",
                FechaNacimiento =
                    DateTime.Today.AddYears(-18),

                // Todo usuario nuevo queda activo
                // desde el momento de su creación.
                Activo = true
            };

            return View(model);
        }

        // POST: Administracion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            UsuarioViewModel model)
        {
            if (string.IsNullOrWhiteSpace(
                model.Password))
            {
                ModelState.AddModelError(
                    "Password",
                    "La contraseña es obligatoria.");
            }

            if (!ModelState.IsValid)
            {
                PrepararRoles(model.Rol);
                return View(model);
            }

            // Los usuarios creados por el administrador
            // quedan activos inicialmente.
            model.Activo = true;

            ResultadoOperacion resultado =
                _usuarioService.Crear(
                    model.AEntidad(),
                    model.Password,
                    model.Rol);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                PrepararRoles(model.Rol);
                return View(model);
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction("Index");
        }

        // GET: Administracion/Edit/id
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            ApplicationUser usuario =
                _usuarioService.ObtenerPorId(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            string rol =
                _usuarioService.ObtenerRol(id);

            UsuarioViewModel model =
                UsuarioViewModel.DesdeEntidad(
                    usuario,
                    rol);

            PrepararRoles(model.Rol);

            return View(model);
        }

        // POST: Administracion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PrepararRoles(model.Rol);
                return View(model);
            }

            ResultadoOperacion resultado =
                _usuarioService.Actualizar(
                    model.AEntidad(),
                    model.Rol);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                PrepararRoles(model.Rol);
                return View(model);
            }

            TempData["MensajeExito"] =
                resultado.Mensaje;

            return RedirectToAction("Index");
        }

        // GET: Administracion/Delete/id
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
            }

            ApplicationUser usuario =
                _usuarioService.ObtenerPorId(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.Rol =
                _usuarioService.ObtenerRol(id);

            return View(usuario);
        }

        // POST: Administracion/Delete/id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == User.Identity.GetUserId())
            {
                TempData["MensajeError"] =
                    "No puede eliminar su propia cuenta " +
                    "mientras está autenticado.";

                return RedirectToAction("Index");
            }

            ResultadoOperacion resultado =
                _usuarioService.Eliminar(id);

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

        private void PrepararRoles(
            string rolSeleccionado)
        {
            ViewBag.Roles =
                new SelectList(
                    _roles,
                    rolSeleccionado);
        }

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _usuarioService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
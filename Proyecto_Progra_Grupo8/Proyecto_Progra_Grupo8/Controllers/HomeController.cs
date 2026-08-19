using Proyecto_Progra_Grupo8.Negocio.Services;
using Proyecto_Progra_Grupo8.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEventoService _eventoService;
        private readonly ICompraService _compraService;

        public HomeController(
            IUsuarioService usuarioService,
            IEventoService eventoService,
            ICompraService compraService)
        {
            if (usuarioService == null)
            {
                throw new ArgumentNullException(
                    nameof(usuarioService));
            }

            if (eventoService == null)
            {
                throw new ArgumentNullException(
                    nameof(eventoService));
            }

            if (compraService == null)
            {
                throw new ArgumentNullException(
                    nameof(compraService));
            }

            _usuarioService = usuarioService;
            _eventoService = eventoService;
            _compraService = compraService;
        }

        // Pública: landing page.
        public ActionResult Index()
        {
            return View();
        }

        // Requiere sesión iniciada, sin importar el rol.
        [Authorize]
        public ActionResult Panel()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Dashboard()
        {
            var eventos =
                _eventoService.ObtenerTodos().ToList();

            var usuarios =
                _usuarioService.ObtenerTodos().ToList();

            DateTime ahora = DateTime.Now;

            var modelo = new DashboardViewModel
            {
                TotalUsuarios = usuarios.Count,

                TotalEventosActivos =
                    eventos.Count(e => e.Activo),

                TotalEventosProximos =
                    eventos.Count(e =>
                        e.Activo &&
                        e.FechaHora > ahora),

                TotalOrdenes =
                    _compraService.ContarOrdenes(),

                EntradasVendidas =
                    _compraService.ContarEntradasVendidas(),

                IngresosTotales =
                    _compraService.ObtenerIngresosTotales(),

                EventosBajoAforo =
                    eventos
                        .Where(e =>
                            e.Activo &&
                            e.FechaHora > ahora &&
                            e.AforoTotal > 0 &&
                            e.EntradasDisponibles <=
                            e.AforoTotal * 0.20m)
                        .OrderBy(e =>
                            e.EntradasDisponibles)
                        .ToList(),

                IngresosPorEvento =
                    _compraService
                        .ObtenerIngresosPorEvento()
            };

            return View(modelo);
        }

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _usuarioService.Dispose();
                _eventoService.Dispose();
                _compraService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
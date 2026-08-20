using System;
using System.Linq;
using System.Web.Http;
using Proyecto_Progra_Grupo8.Models.Api;
using Proyecto_Progra_Grupo8.Negocio.Services;

namespace Proyecto_Progra_Grupo8.Controllers.Api
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/dashboard")]
    public class DashboardApiController : ApiController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEventoService _eventoService;
        private readonly ICompraService _compraService;

        public DashboardApiController(
            IUsuarioService usuarioService,
            IEventoService eventoService,
            ICompraService compraService)
        {
            _usuarioService = usuarioService;
            _eventoService = eventoService;
            _compraService = compraService;
        }

        // GET api/dashboard
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var eventos =
                _eventoService.ObtenerTodos().ToList();

            var usuarios =
                _usuarioService.ObtenerTodos().ToList();

            DateTime ahora = DateTime.Now;

            var modelo = new DashboardApiDto
            {
                TotalUsuarios = usuarios.Count,

                UsuariosActivos = usuarios.Count(u => u.Activo),

                UsuariosInactivos = usuarios.Count(u => !u.Activo),

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

                EventosBajoAforo = eventos
                    .Where(e =>
                        e.Activo &&
                        e.FechaHora > ahora &&
                        e.AforoTotal > 0 &&
                        e.EntradasDisponibles <=
                        e.AforoTotal * 0.20m)
                    .OrderBy(e => e.EntradasDisponibles)
                    .Select(e => new EventoBajoAforoDto
                    {
                        EventoId = e.EventoId,
                        Nombre = e.Nombre,
                        FechaHora = e.FechaHora,
                        EntradasDisponibles = e.EntradasDisponibles,
                        AforoTotal = e.AforoTotal
                    })
                    .ToList(),

                IngresosPorEvento =
                    _compraService.ObtenerIngresosPorEvento()
            };

            return Ok(modelo);
        }

        protected override void Dispose(bool disposing)
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

using System.Web.Http;
using Proyecto_Progra_Grupo8.Negocio.Services;

namespace Proyecto_Progra_Grupo8.Controllers.Api
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/eventos")]
    public class EventosApiController : ApiController
    {
        private readonly IEventoService _eventoService;

        public EventosApiController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // GET api/eventos/codigo-disponible?codigo=CONC-01&eventoId=0
        [HttpGet]
        [Route("codigo-disponible")]
        public IHttpActionResult CodigoDisponible(
            string codigo,
            int eventoId = 0)
        {
            bool disponible = _eventoService.CodigoDisponible(
                codigo,
                eventoId);

            return Ok(new { disponible });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventoService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

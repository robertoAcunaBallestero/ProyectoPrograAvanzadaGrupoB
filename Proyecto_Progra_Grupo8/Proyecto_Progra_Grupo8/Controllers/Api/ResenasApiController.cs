using System.Net;
using System.Web.Http;
using Proyecto_Progra_Grupo8.Filters;
using Proyecto_Progra_Grupo8.Models.Api;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using Proyecto_Progra_Grupo8.Negocio.Services;

namespace Proyecto_Progra_Grupo8.Controllers.Api
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/resenas")]
    public class ResenasApiController : ApiController
    {
        private readonly IResenaService _resenaService;

        public ResenasApiController(IResenaService resenaService)
        {
            _resenaService = resenaService;
        }

        // PUT api/resenas/5
        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public IHttpActionResult Put(int id, ModerarResenaDto dto)
        {
            if (_resenaService.ObtenerPorId(id) == null)
            {
                return NotFound();
            }

            ResultadoOperacion resultado = dto.Estado == "Aprobada"
                ? _resenaService.Aprobar(id)
                : _resenaService.Rechazar(id);

            var respuesta = new OperacionApiDto(
                resultado.Exito,
                resultado.Mensaje);

            if (!resultado.Exito)
            {
                return Content(HttpStatusCode.BadRequest, respuesta);
            }

            return Ok(respuesta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _resenaService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

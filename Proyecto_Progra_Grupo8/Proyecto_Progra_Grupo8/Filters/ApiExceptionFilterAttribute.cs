using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Proyecto_Progra_Grupo8.Filters
{
    // Manejo centralizado de excepciones para todos los controladores
    // de la Web API: evita exponer detalles internos y responde JSON uniforme.
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Trace.TraceError(context.Exception.ToString());

            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                new
                {
                    exito = false,
                    mensaje = "Ocurrió un error interno al procesar la solicitud."
                });
        }
    }
}

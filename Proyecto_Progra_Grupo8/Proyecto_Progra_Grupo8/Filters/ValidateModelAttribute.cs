using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Proyecto_Progra_Grupo8.Filters
{
    // Valida las Data Annotations del DTO de entrada antes de ejecutar
    // la acción del controlador; corta con 400 si el modelo es inválido.
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errores = actionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                actionContext.Response =
                    actionContext.Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new
                        {
                            exito = false,
                            mensaje = errores.Any()
                                ? string.Join(" ", errores)
                                : "Datos inválidos."
                        });
            }
        }
    }
}

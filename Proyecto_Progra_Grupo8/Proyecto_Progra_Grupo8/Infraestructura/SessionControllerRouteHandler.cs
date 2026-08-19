using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace Proyecto_Progra_Grupo8.Infraestructura
{
    // Habilita System.Web.SessionState en rutas de Web API puntuales
    // (Web API no tiene sesión disponible por defecto).
    public class SessionControllerHandler
        : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        {
        }
    }

    public class SessionControllerRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(
            RequestContext requestContext)
        {
            return new SessionControllerHandler(
                requestContext.RouteData);
        }
    }
}

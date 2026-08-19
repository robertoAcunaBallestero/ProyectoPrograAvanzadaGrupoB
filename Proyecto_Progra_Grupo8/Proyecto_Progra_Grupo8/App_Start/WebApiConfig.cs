using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Proyecto_Progra_Grupo8.Filters;
using Proyecto_Progra_Grupo8.Infraestructura;

namespace Proyecto_Progra_Grupo8
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Ruta con sesión habilitada (el carrito vive en Session):
            // se registra directamente en RouteTable.Routes -no en
            // config.Routes- porque solo un System.Web.Routing.Route
            // permite asignar un RouteHandler personalizado, y debe ir
            // ANTES que "DefaultApi" para no ser eclipsada por ella
            // (si no, "api/carrito/agregar" matchearía primero contra
            // api/{controller}/{id} con controller=carrito, id=agregar).
            RouteTable.Routes.MapHttpRoute(
                name: "CarritoApi",
                routeTemplate: "api/carrito/{action}",
                defaults: new { controller = "CarritoApi" })
                .RouteHandler = new SessionControllerRouteHandler();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Filters.Add(new ApiExceptionFilterAttribute());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings
                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // camelCase en el JSON de salida (totalUsuarios en vez de
            // TotalUsuarios) para que se consuma de forma natural desde JS.
            config.Formatters.JsonFormatter.SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}

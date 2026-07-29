using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
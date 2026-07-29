using Microsoft.AspNet.Identity.Owin;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    // Solo accesible para usuarios en el rol "Administrador".
    [Authorize(Roles = "Administrador")]
    public class AdministracionController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Administracion
        public ActionResult Index()
        {
            var asociados = UserManager.Users
                .OfType<ApplicationUser>()
                .OrderBy(u => u.NombreCompleto)
                .ToList();

            return View(asociados);
        }
    }
}

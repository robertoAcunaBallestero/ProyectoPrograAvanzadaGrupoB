using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Proyecto_Progra_Grupo8.Models;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Unity;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [InjectionConstructor]
        public AccountController()
        {
        }

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ??
                    HttpContext
                        .GetOwinContext()
                        .Get<ApplicationSignInManager>();
            }

            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext
                        .GetOwinContext()
                        .GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(
            LoginViewModel model,
            string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Primero buscamos al usuario para comprobar
            // si su cuenta se encuentra activa.
            ApplicationUser usuario =
                await UserManager.FindByEmailAsync(model.Email);

            if (usuario != null && !usuario.Activo)
            {
                ModelState.AddModelError(
                    "",
                    "Su cuenta se encuentra inactiva. " +
                    "Debe comunicarse con un administrador.");

                return View(model);
            }

            var result =
                await SignInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:

                    // Registra automáticamente la fecha y hora
                    // del último inicio de sesión exitoso.
                    if (usuario != null)
                    {
                        usuario.UltimaConexion = DateTime.Now;

                        await UserManager.UpdateAsync(usuario);
                    }

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError(
                        "",
                        "Intento de inicio de sesión no válido.");

                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                Cedula = model.Cedula,
                NumeroAsociado = model.NumeroAsociado,
                FechaNacimiento = model.FechaNacimiento,
                FechaIngreso = DateTime.Now,

                // Todo usuario nuevo queda activo.
                Activo = true
            };

            var result =
                await UserManager.CreateAsync(
                    user,
                    model.Password);

            if (result.Succeeded)
            {
                // Todo usuario que se autorregistra entra
                // con el rol Asociado.
                await UserManager.AddToRoleAsync(
                    user.Id,
                    "Asociado");

                // Como el registro inicia sesión automáticamente,
                // también registramos la primera conexión.
                user.UltimaConexion = DateTime.Now;

                await UserManager.UpdateAsync(user);

                await SignInManager.SignInAsync(
                    user,
                    isPersistent: false,
                    rememberBrowser: false);

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error);
            }

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes
                    .ApplicationCookie);

            return RedirectToAction(
                "Index",
                "Home");
        }

        // GET: /Account/Lockout
        [AllowAnonymous]
        public ActionResult Lockout()
        {
            return View();
        }

        // GET: /Account/Perfil
        public ActionResult Perfil()
        {
            string usuarioId =
                User.Identity.GetUserId();

            ApplicationUser usuario =
                UserManager.FindById(usuarioId);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext
                    .GetOwinContext()
                    .Authentication;
            }
        }

        private ActionResult RedirectToLocal(
            string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ProyectoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioRepository(ProyectoDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;

            var userStore =
                new UserStore<ApplicationUser>(_context)
                {
                    DisposeContext = false
                };

            var roleStore =
                new RoleStore<IdentityRole>(_context)
                {
                    DisposeContext = false
                };

            _userManager =
                new UserManager<ApplicationUser>(userStore);

            _roleManager =
                new RoleManager<IdentityRole>(roleStore);

            _userManager.UserValidator =
                new UserValidator<ApplicationUser>(
                    _userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

            _userManager.PasswordValidator =
                new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true
                };
        }

        public IEnumerable<ApplicationUser> ObtenerTodos()
        {
            return _userManager.Users
                .OrderBy(u => u.NombreCompleto)
                .ToList();
        }

        public ApplicationUser ObtenerPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _userManager.FindById(id);
        }

        public string ObtenerRol(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return null;
            }

            return _userManager
                .GetRoles(usuarioId)
                .FirstOrDefault();
        }

        public bool ExisteCorreo(
            string correo,
            string usuarioIdExcluido)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                return false;
            }

            string correoNormalizado =
                correo.Trim().ToLower();

            return _userManager.Users.Any(u =>
                u.Email.ToLower() == correoNormalizado &&
                u.Id != usuarioIdExcluido);
        }

        public bool ExisteCedula(
            string cedula,
            string usuarioIdExcluido)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return false;
            }

            string cedulaNormalizada =
                cedula.Trim();

            return _userManager.Users.Any(u =>
                u.Cedula == cedulaNormalizada &&
                u.Id != usuarioIdExcluido);
        }

        public bool ExisteNumeroAsociado(
            string numeroAsociado,
            string usuarioIdExcluido)
        {
            if (string.IsNullOrWhiteSpace(
                numeroAsociado))
            {
                return false;
            }

            string numeroNormalizado =
                numeroAsociado.Trim().ToUpper();

            return _userManager.Users.Any(u =>
                u.NumeroAsociado == numeroNormalizado &&
                u.Id != usuarioIdExcluido);
        }

        public IdentityResult Crear(
            ApplicationUser usuario,
            string contrasena,
            string rol)
        {
            IdentityResult resultadoRol =
                AsegurarRol(rol);

            if (!resultadoRol.Succeeded)
            {
                return resultadoRol;
            }

            IdentityResult resultado =
                _userManager.Create(
                    usuario,
                    contrasena);

            if (!resultado.Succeeded)
            {
                return resultado;
            }

            resultado =
                _userManager.AddToRole(
                    usuario.Id,
                    rol);

            if (!resultado.Succeeded)
            {
                // Evita dejar un usuario creado sin rol.
                _userManager.Delete(usuario);
            }

            return resultado;
        }

        public IdentityResult Actualizar(
            ApplicationUser usuario,
            string rol)
        {
            ApplicationUser existente =
                _userManager.FindById(usuario.Id);

            if (existente == null)
            {
                return IdentityResult.Failed(
                    "El usuario indicado no existe.");
            }

            existente.NombreCompleto =
                usuario.NombreCompleto;

            existente.Cedula =
                usuario.Cedula;

            existente.FechaNacimiento =
                usuario.FechaNacimiento;

            existente.NumeroAsociado =
                usuario.NumeroAsociado;

            existente.Email =
                usuario.Email;

            existente.UserName =
                usuario.Email;

            IdentityResult resultado =
                _userManager.Update(existente);

            if (!resultado.Succeeded)
            {
                return resultado;
            }

            resultado =
                AsegurarRol(rol);

            if (!resultado.Succeeded)
            {
                return resultado;
            }

            string[] rolesActuales =
                _userManager
                    .GetRoles(existente.Id)
                    .ToArray();

            if (rolesActuales.Length == 1 &&
                rolesActuales[0] == rol)
            {
                return resultado;
            }

            if (rolesActuales.Length > 0)
            {
                resultado =
                    _userManager.RemoveFromRoles(
                        existente.Id,
                        rolesActuales);

                if (!resultado.Succeeded)
                {
                    return resultado;
                }
            }

            return _userManager.AddToRole(
                existente.Id,
                rol);
        }

        public IdentityResult Eliminar(string id)
        {
            ApplicationUser usuario =
                _userManager.FindById(id);

            if (usuario == null)
            {
                return IdentityResult.Failed(
                    "El usuario indicado no existe.");
            }

            return _userManager.Delete(usuario);
        }

        private IdentityResult AsegurarRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol))
            {
                return IdentityResult.Failed(
                    "El rol es obligatorio.");
            }

            if (_roleManager.RoleExists(rol))
            {
                return IdentityResult.Success;
            }

            return _roleManager.Create(
                new IdentityRole(rol));
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _roleManager.Dispose();
        }
    }
}

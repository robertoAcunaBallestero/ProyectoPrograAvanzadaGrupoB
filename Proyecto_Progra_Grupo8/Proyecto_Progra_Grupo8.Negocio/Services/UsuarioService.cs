using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository)
        {
            if (usuarioRepository == null)
            {
                throw new ArgumentNullException(
                    nameof(usuarioRepository));
            }

            _usuarioRepository = usuarioRepository;
        }

        public IEnumerable<ApplicationUser> ObtenerTodos()
        {
            return _usuarioRepository.ObtenerTodos();
        }

        public ApplicationUser ObtenerPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _usuarioRepository.ObtenerPorId(id);
        }

        public string ObtenerRol(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return null;
            }

            return _usuarioRepository.ObtenerRol(usuarioId);
        }

        public ResultadoOperacion Crear(
            ApplicationUser usuario,
            string contrasena,
            string rol)
        {
            ResultadoOperacion validacion =
                ValidarUsuario(usuario, rol);

            if (!validacion.Exito)
            {
                return validacion;
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                return new ResultadoOperacion(
                    false,
                    "La contraseña es obligatoria.");
            }

            NormalizarDatos(usuario);

            if (_usuarioRepository.ExisteCorreo(
                usuario.Email,
                null))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe un usuario con ese correo electrónico.");
            }

            if (_usuarioRepository.ExisteCedula(
                usuario.Cedula,
                null))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe un usuario con esa cédula.");
            }

            if (_usuarioRepository.ExisteNumeroAsociado(
                usuario.NumeroAsociado,
                null))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe un usuario con ese número de asociado.");
            }

            usuario.UserName = usuario.Email;
            usuario.FechaIngreso = DateTime.Now;

            IdentityResult resultado =
                _usuarioRepository.Crear(
                    usuario,
                    contrasena,
                    rol);

            if (!resultado.Succeeded)
            {
                return new ResultadoOperacion(
                    false,
                    ObtenerErrores(resultado));
            }

            return new ResultadoOperacion(
                true,
                "El usuario fue creado correctamente.",
                usuario);
        }

        public ResultadoOperacion Actualizar(
            ApplicationUser usuario,
            string rol)
        {
            if (usuario == null ||
                string.IsNullOrWhiteSpace(usuario.Id))
            {
                return new ResultadoOperacion(
                    false,
                    "El usuario indicado no es válido.");
            }

            ResultadoOperacion validacion =
                ValidarUsuario(usuario, rol);

            if (!validacion.Exito)
            {
                return validacion;
            }

            ApplicationUser usuarioExistente =
                _usuarioRepository.ObtenerPorId(
                    usuario.Id);

            if (usuarioExistente == null)
            {
                return new ResultadoOperacion(
                    false,
                    "El usuario indicado no existe.");
            }

            NormalizarDatos(usuario);

            if (_usuarioRepository.ExisteCorreo(
                usuario.Email,
                usuario.Id))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe otro usuario con ese correo electrónico.");
            }

            if (_usuarioRepository.ExisteCedula(
                usuario.Cedula,
                usuario.Id))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe otro usuario con esa cédula.");
            }

            if (_usuarioRepository.ExisteNumeroAsociado(
                usuario.NumeroAsociado,
                usuario.Id))
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe otro usuario con ese número de asociado.");
            }

            usuario.UserName = usuario.Email;

            IdentityResult resultado =
                _usuarioRepository.Actualizar(
                    usuario,
                    rol);

            if (!resultado.Succeeded)
            {
                return new ResultadoOperacion(
                    false,
                    ObtenerErrores(resultado));
            }

            return new ResultadoOperacion(
                true,
                "El usuario fue actualizado correctamente.",
                usuario);
        }

        public ResultadoOperacion Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new ResultadoOperacion(
                    false,
                    "El identificador del usuario no es válido.");
            }

            ApplicationUser usuario =
                _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return new ResultadoOperacion(
                    false,
                    "El usuario indicado no existe.");
            }

            IdentityResult resultado =
                _usuarioRepository.Eliminar(id);

            if (!resultado.Succeeded)
            {
                return new ResultadoOperacion(
                    false,
                    ObtenerErrores(resultado));
            }

            return new ResultadoOperacion(
                true,
                "El usuario fue eliminado correctamente.");
        }

        private ResultadoOperacion ValidarUsuario(
            ApplicationUser usuario,
            string rol)
        {
            if (usuario == null)
            {
                return new ResultadoOperacion(
                    false,
                    "Debe proporcionar los datos del usuario.");
            }

            if (string.IsNullOrWhiteSpace(
                usuario.NombreCompleto))
            {
                return new ResultadoOperacion(
                    false,
                    "El nombre completo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                usuario.Cedula))
            {
                return new ResultadoOperacion(
                    false,
                    "La cédula es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(
                usuario.Email))
            {
                return new ResultadoOperacion(
                    false,
                    "El correo electrónico es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                usuario.NumeroAsociado))
            {
                return new ResultadoOperacion(
                    false,
                    "El número de asociado es obligatorio.");
            }

            if (usuario.FechaNacimiento ==
                default(DateTime))
            {
                return new ResultadoOperacion(
                    false,
                    "La fecha de nacimiento es obligatoria.");
            }

            if (usuario.FechaNacimiento >=
                DateTime.Today)
            {
                return new ResultadoOperacion(
                    false,
                    "La fecha de nacimiento no es válida.");
            }

            if (string.IsNullOrWhiteSpace(rol))
            {
                return new ResultadoOperacion(
                    false,
                    "El rol es obligatorio.");
            }

            return new ResultadoOperacion(
                true,
                "Los datos del usuario son válidos.");
        }

        private void NormalizarDatos(
            ApplicationUser usuario)
        {
            usuario.NombreCompleto =
                usuario.NombreCompleto.Trim();

            usuario.Cedula =
                usuario.Cedula.Trim();

            usuario.Email =
                usuario.Email
                    .Trim()
                    .ToLowerInvariant();

            usuario.NumeroAsociado =
                usuario.NumeroAsociado
                    .Trim()
                    .ToUpperInvariant();
        }

        private string ObtenerErrores(
            IdentityResult resultado)
        {
            if (resultado == null ||
                resultado.Errors == null)
            {
                return "Ocurrió un error al procesar el usuario.";
            }

            string mensaje =
                string.Join(
                    " ",
                    resultado.Errors
                        .Where(error =>
                            !string.IsNullOrWhiteSpace(error)));

            return string.IsNullOrWhiteSpace(mensaje)
                ? "Ocurrió un error al procesar el usuario."
                : mensaje;
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}

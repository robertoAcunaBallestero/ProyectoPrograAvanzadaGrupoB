using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public interface IUsuarioRepository : IDisposable
    {
        IEnumerable<ApplicationUser> ObtenerTodos();

        ApplicationUser ObtenerPorId(string id);

        string ObtenerRol(string usuarioId);

        bool ExisteCorreo(
            string correo,
            string usuarioIdExcluido);

        bool ExisteCedula(
            string cedula,
            string usuarioIdExcluido);

        bool ExisteNumeroAsociado(
            string numeroAsociado,
            string usuarioIdExcluido);

        IdentityResult Crear(
            ApplicationUser usuario,
            string contrasena,
            string rol);

        IdentityResult Actualizar(
            ApplicationUser usuario,
            string rol);

        IdentityResult Eliminar(string id);
    }
}

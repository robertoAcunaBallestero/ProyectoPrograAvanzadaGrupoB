using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{

    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(
            120,
            MinimumLength = 3,
            ErrorMessage = "El nombre completo debe contener entre 3 y 120 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(
            20,
            MinimumLength = 9,
            ErrorMessage = "La cédula debe tener entre 9 y 20 caracteres.")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El número de asociado es obligatorio.")]
        [StringLength(
            7,
            MinimumLength = 7,
            ErrorMessage = "El número de asociado debe tener 7 caracteres.")]
        [RegularExpression(
            @"^AS-\d{4}$",
            ErrorMessage = "El número de asociado debe tener el formato AS-0000.")]
        [Display(Name = "Número de asociado")]
        public string NumeroAsociado { get; set; }

        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        // Genera la identidad utilizada por la cookie de autenticación.
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                DefaultAuthenticationTypes.ApplicationCookie);

            // Permite acceder al nombre completo y al número de asociado
            // desde los datos del usuario autenticado.
            userIdentity.AddClaim(
                new Claim(
                    "NombreCompleto",
                    NombreCompleto ?? string.Empty));

            userIdentity.AddClaim(
                new Claim(
                    "NumeroAsociado",
                    NumeroAsociado ?? string.Empty));

            return userIdentity;
        }
    }
}

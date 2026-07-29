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
        [StringLength(120, MinimumLength = 3)]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20, MinimumLength = 9,
            ErrorMessage = "La cédula debe tener entre 9 y 20 caracteres.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El número de asociado es obligatorio.")]
        [RegularExpression(@"^AS-\d{4}$",
            ErrorMessage = "El número de asociado debe tener el formato AS-0000.")]
        public string NumeroAsociado { get; set; }

        public DateTime FechaIngreso { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType debe coincidir con el valor definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("NombreCompleto", this.NombreCompleto ?? string.Empty));
            userIdentity.AddClaim(new Claim("NumeroAsociado", this.NumeroAsociado ?? string.Empty));
            return userIdentity;
        }
    }
}

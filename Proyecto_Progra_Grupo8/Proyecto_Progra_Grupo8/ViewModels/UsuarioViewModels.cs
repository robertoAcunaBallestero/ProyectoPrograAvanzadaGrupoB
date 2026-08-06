using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Progra_Grupo8.ViewModels
{
    public class UsuarioViewModel : IValidatableObject
    {
        public string Id { get; set; }

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

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El número de asociado es obligatorio.")]
        [RegularExpression(
            @"^AS-\d{4}$",
            ErrorMessage = "El número de asociado debe tener el formato AS-0000.")]
        [Display(Name = "Número de asociado")]
        public string NumeroAsociado { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [StringLength(
            100,
            MinimumLength = 6,
            ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare(
            "Password",
            ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }

        public ApplicationUser AEntidad()
        {
            return new ApplicationUser
            {
                Id = Id,
                NombreCompleto = NombreCompleto,
                Cedula = Cedula,
                Email = Email,
                UserName = Email,
                NumeroAsociado = NumeroAsociado,
                FechaNacimiento = FechaNacimiento
            };
        }

        public static UsuarioViewModel DesdeEntidad(
            ApplicationUser usuario,
            string rol)
        {
            if (usuario == null)
            {
                return null;
            }

            return new UsuarioViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Cedula = usuario.Cedula,
                Email = usuario.Email,
                NumeroAsociado = usuario.NumeroAsociado,
                FechaNacimiento = usuario.FechaNacimiento,
                Rol = rol
            };
        }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (FechaNacimiento == default(DateTime))
            {
                yield break;
            }

            int edad =
                DateTime.Today.Year -
                FechaNacimiento.Year;

            if (FechaNacimiento.Date >
                DateTime.Today.AddYears(-edad))
            {
                edad--;
            }

            if (edad < 18)
            {
                yield return new ValidationResult(
                    "El asociado debe ser mayor de edad.",
                    new[] { nameof(FechaNacimiento) });
            }
        }
    }
}
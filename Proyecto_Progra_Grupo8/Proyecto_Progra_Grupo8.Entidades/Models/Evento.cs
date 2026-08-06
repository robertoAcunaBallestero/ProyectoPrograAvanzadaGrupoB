using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Progra_Grupo8.Entidades.Models
{
    public class Evento
    {
        public Evento()
        {
            Imagenes = new HashSet<ImagenEvento>();
        }

        public int EventoId { get; set; }

        [Required(ErrorMessage = "El código del evento es obligatorio.")]
        [StringLength(
            20,
            ErrorMessage = "El código no puede superar los 20 caracteres.")]
        [Display(Name = "Código")]
        public string CodigoEvento { get; set; }

        [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
        [StringLength(
            150,
            MinimumLength = 3,
            ErrorMessage = "El nombre debe contener entre 3 y 150 caracteres.")]
        [Display(Name = "Nombre del evento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(
            1000,
            ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría")]
        public int CategoriaEventoId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El lugar es obligatorio.")]
        [StringLength(
            200,
            ErrorMessage = "El lugar no puede superar los 200 caracteres.")]
        [Display(Name = "Lugar")]
        public string Lugar { get; set; }

        [Required(ErrorMessage = "El precio de la entrada es obligatorio.")]
        [Range(
            0,
            99999999,
            ErrorMessage = "El precio no puede ser negativo.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio de la entrada")]
        public decimal PrecioEntrada { get; set; }

        [Required(ErrorMessage = "El aforo total es obligatorio.")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "El aforo total debe ser mayor que cero.")]
        [Display(Name = "Aforo total")]
        public int AforoTotal { get; set; }

        [Required(ErrorMessage = "Las entradas disponibles son obligatorias.")]
        [Range(
            0,
            int.MaxValue,
            ErrorMessage = "Las entradas disponibles no pueden ser negativas.")]
        [Display(Name = "Entradas disponibles")]
        public int EntradasDisponibles { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual CategoriaEvento CategoriaEvento { get; set; }

        public virtual ICollection<ImagenEvento> Imagenes { get; set; }
    }
}

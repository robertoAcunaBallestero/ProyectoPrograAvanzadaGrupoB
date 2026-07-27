using Proyecto_Progra_Grupo8.Entidades.Models;
using System.Data.Entity;

namespace Proyecto_Progra_Grupo8.Datos
{
    public class ProyectoDbContext : DbContext
    {
        public ProyectoDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Evento> Eventos { get; set; }

        // ¡Aquí agregamos la tabla de imágenes!
        public DbSet<ImagenEvento> ImagenesEventos { get; set; }

        public static ProyectoDbContext Create()
        {
            return new ProyectoDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evento>()
                .Property(e => e.PrecioEntrada)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Evento>()
                .Property(e => e.CodigoEvento)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Evento>()
                .Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Evento>()
                .ToTable("Eventos");

            
            modelBuilder.Entity<ImagenEvento>()
                .HasRequired(i => i.Evento)
                .WithMany(e => e.Imagenes) 
                .HasForeignKey(i => i.EventoId)
                .WillCascadeOnDelete(true);
        }
    }
}
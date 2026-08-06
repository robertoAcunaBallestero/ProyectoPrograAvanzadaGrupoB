using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System.Data.Entity;

namespace Proyecto_Progra_Grupo8.Datos
{
    public class ProyectoDbContext : IdentityDbContext<ApplicationUser>
    {
        static ProyectoDbContext()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<
                    ProyectoDbContext,
                    Migrations.Configuration>());
        }

        public ProyectoDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<CategoriaEvento> CategoriasEvento { get; set; }

        public DbSet<Evento> Eventos { get; set; }

        public DbSet<ImagenEvento> ImagenesEventos { get; set; }

        public DbSet<Orden> Ordenes { get; set; }

        public DbSet<DetalleOrden> DetallesOrden { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public static ProyectoDbContext Create()
        {
            return new ProyectoDbContext();
        }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nombres de las tablas
            modelBuilder.Entity<CategoriaEvento>()
                .ToTable("CategoriasEvento");

            modelBuilder.Entity<Evento>()
                .ToTable("Eventos");

            modelBuilder.Entity<ImagenEvento>()
                .ToTable("ImagenEventoes");

            modelBuilder.Entity<Orden>()
                .ToTable("Ordenes");

            modelBuilder.Entity<DetalleOrden>()
                .ToTable("DetallesOrden");

            modelBuilder.Entity<Ticket>()
                .ToTable("Tickets");

            // Precisión de valores monetarios
            modelBuilder.Entity<Evento>()
                .Property(e => e.PrecioEntrada)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Orden>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleOrden>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            // Una categoría puede tener muchos eventos
            modelBuilder.Entity<Evento>()
                .HasRequired(e => e.CategoriaEvento)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.CategoriaEventoId)
                .WillCascadeOnDelete(false);

            // Un evento puede tener muchas imágenes
            modelBuilder.Entity<ImagenEvento>()
                .HasRequired(i => i.Evento)
                .WithMany(e => e.Imagenes)
                .HasForeignKey(i => i.EventoId)
                .WillCascadeOnDelete(true);

            // Una orden puede tener muchos detalles
            modelBuilder.Entity<DetalleOrden>()
                .HasRequired(d => d.Orden)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenId)
                .WillCascadeOnDelete(true);

            // Un evento puede aparecer en muchos detalles de orden
            modelBuilder.Entity<DetalleOrden>()
                .HasRequired(d => d.Evento)
                .WithMany()
                .HasForeignKey(d => d.EventoId)
                .WillCascadeOnDelete(false);

            // Un detalle de orden puede tener muchos tickets
            modelBuilder.Entity<Ticket>()
                .HasRequired(t => t.DetalleOrden)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DetalleOrdenId)
                .WillCascadeOnDelete(true);
        }
    }
}
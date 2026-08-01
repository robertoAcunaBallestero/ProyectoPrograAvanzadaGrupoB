using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Datos
{
    public class ComprasDbContext : DbContext
    {
        static ComprasDbContext() { Database.SetInitializer<ComprasDbContext>(null); }

        public ComprasDbContext() : base("DefaultConnection") { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<ImagenEvento> ImagenesEventos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetallesOrden { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>().ToTable("Eventos");
            modelBuilder.Entity<ImagenEvento>().ToTable("ImagenEventoes");
            modelBuilder.Entity<Orden>().ToTable("Ordenes");
            modelBuilder.Entity<DetalleOrden>().ToTable("DetallesOrden");
            modelBuilder.Entity<Ticket>().ToTable("Tickets");

            modelBuilder.Entity<Evento>().Property(e => e.PrecioEntrada).HasPrecision(18, 2);
            modelBuilder.Entity<Orden>().Property(o => o.Total).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleOrden>().Property(d => d.PrecioUnitario).HasPrecision(18, 2);

            modelBuilder.Entity<ImagenEvento>()
                .HasRequired(i => i.Evento).WithMany().HasForeignKey(i => i.EventoId).WillCascadeOnDelete(true);
            modelBuilder.Entity<DetalleOrden>()
                .HasRequired(d => d.Orden).WithMany(o => o.Detalles).HasForeignKey(d => d.OrdenId).WillCascadeOnDelete(true);
            modelBuilder.Entity<DetalleOrden>()
                .HasRequired(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Ticket>()
                .HasRequired(t => t.DetalleOrden).WithMany(d => d.Tickets).HasForeignKey(t => t.DetalleOrdenId).WillCascadeOnDelete(true);
        }
        }
}

namespace Proyecto_Progra_Grupo8.Datos.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Proyecto_Progra_Grupo8.Entidades.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration :
        DbMigrationsConfiguration<
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            CrearRolesYUsuarios(context);
            CrearCategorias(context);
            CrearEventos(context);
            CrearImagenesEventos(context);

            context.SaveChanges();
        }

        private void CrearRolesYUsuarios(
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            foreach (var rol in new[]
            {
                "Administrador",
                "Asociado"
            })
            {
                if (!roleManager.RoleExists(rol))
                {
                    roleManager.Create(
                        new IdentityRole(rol));
                }
            }

            var userManager =
                new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            const string adminEmail =
                "admin@cooperativa.cr";

            ApplicationUser administrador =
                userManager.FindByName(adminEmail);

            if (administrador == null)
            {
                administrador =
                    new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        NombreCompleto =
                            "Administrador Cooperativa",
                        Cedula = "000000000",
                        FechaNacimiento =
                            new DateTime(1990, 1, 1),
                        NumeroAsociado = "AS-0001",
                        FechaIngreso = DateTime.Now,
                        EmailConfirmed = true
                    };

                IdentityResult resultado =
                    userManager.Create(
                        administrador,
                        "Admin123!");

                if (resultado.Succeeded)
                {
                    userManager.AddToRole(
                        administrador.Id,
                        "Administrador");
                }
            }

            const string asociadoEmail =
                "asociado@cooperativa.cr";

            ApplicationUser asociado =
                userManager.FindByName(asociadoEmail);

            if (asociado == null)
            {
                asociado =
                    new ApplicationUser
                    {
                        UserName = asociadoEmail,
                        Email = asociadoEmail,
                        NombreCompleto =
                            "Asociado de Prueba",
                        Cedula = "111111111",
                        FechaNacimiento =
                            new DateTime(1995, 5, 15),
                        NumeroAsociado = "AS-0002",
                        FechaIngreso = DateTime.Now,
                        EmailConfirmed = true
                    };

                IdentityResult resultado =
                    userManager.Create(
                        asociado,
                        "Asociado123!");

                if (resultado.Succeeded)
                {
                    userManager.AddToRole(
                        asociado.Id,
                        "Asociado");
                }
            }
        }

        private void CrearCategorias(
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            context.CategoriasEvento.AddOrUpdate(
                categoria => categoria.Nombre,

                new CategoriaEvento
                {
                    Nombre = "Concierto",
                    Descripcion =
                        "Eventos musicales y conciertos.",
                    Activa = true
                },

                new CategoriaEvento
                {
                    Nombre = "Taller",
                    Descripcion =
                        "Talleres educativos y actividades prácticas.",
                    Activa = true
                },

                new CategoriaEvento
                {
                    Nombre = "Charla",
                    Descripcion =
                        "Charlas, conferencias y exposiciones.",
                    Activa = true
                },

                new CategoriaEvento
                {
                    Nombre = "Teatro",
                    Descripcion =
                        "Obras de teatro y presentaciones culturales.",
                    Activa = true
                });

            context.SaveChanges();
        }

        private void CrearEventos(
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            CategoriaEvento concierto =
                context.CategoriasEvento
                    .First(c =>
                        c.Nombre == "Concierto");

            CategoriaEvento taller =
                context.CategoriasEvento
                    .First(c =>
                        c.Nombre == "Taller");

            CategoriaEvento charla =
                context.CategoriasEvento
                    .First(c =>
                        c.Nombre == "Charla");

            DateTime fechaBase =
                DateTime.Today
                    .AddDays(30)
                    .AddHours(18);

            context.Eventos.AddOrUpdate(
                evento => evento.CodigoEvento,

                new Evento
                {
                    CodigoEvento = "EVT-001",
                    Nombre = "Concierto de verano",
                    Descripcion =
                        "Concierto para los asociados y sus familias.",
                    CategoriaEventoId =
                        concierto.CategoriaEventoId,
                    FechaHora = fechaBase,
                    Lugar =
                        "Auditorio principal",
                    PrecioEntrada = 7500m,
                    AforoTotal = 200,
                    EntradasDisponibles = 200,
                    Activo = true
                },

                new Evento
                {
                    CodigoEvento = "EVT-002",
                    Nombre =
                        "Taller de herramientas digitales",
                    Descripcion =
                        "Taller introductorio sobre herramientas digitales.",
                    CategoriaEventoId =
                        taller.CategoriaEventoId,
                    FechaHora =
                        fechaBase.AddDays(10),
                    Lugar =
                        "Sala de capacitación",
                    PrecioEntrada = 3500m,
                    AforoTotal = 40,
                    EntradasDisponibles = 40,
                    Activo = true
                },

                new Evento
                {
                    CodigoEvento = "EVT-003",
                    Nombre =
                        "Charla de educación financiera",
                    Descripcion =
                        "Charla sobre ahorro, crédito y planificación financiera.",
                    CategoriaEventoId =
                        charla.CategoriaEventoId,
                    FechaHora =
                        fechaBase.AddDays(20),
                    Lugar =
                        "Salón comunal",
                    PrecioEntrada = 2500m,
                    AforoTotal = 100,
                    EntradasDisponibles = 100,
                    Activo = true
                });

            context.SaveChanges();
        }

        private void CrearImagenesEventos(
            Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            byte[] imagenEjemplo =
                Convert.FromBase64String(
                    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAAB" +
                    "CAQAAAC1HAwCAAAAC0lEQVR42mP8/x8A" +
                    "AusB9Y9Zl9sAAAAASUVORK5CYII=");

            string[] codigosEventos =
            {
                "EVT-001",
                "EVT-002",
                "EVT-003"
            };

            foreach (string codigo in codigosEventos)
            {
                Evento evento =
                    context.Eventos
                        .FirstOrDefault(e =>
                            e.CodigoEvento == codigo);

                if (evento == null)
                {
                    continue;
                }

                int cantidadActual =
                    context.ImagenesEventos.Count(i =>
                        i.EventoId == evento.EventoId);

                int cantidadFaltante =
                    3 - cantidadActual;

                for (int i = 0;
                     i < cantidadFaltante;
                     i++)
                {
                    context.ImagenesEventos.Add(
                        new ImagenEvento
                        {
                            EventoId =
                                evento.EventoId,
                            Archivo =
                                imagenEjemplo,
                            TipoContenido =
                                "image/png"
                        });
                }
            }

            context.SaveChanges();
        }
    }
}

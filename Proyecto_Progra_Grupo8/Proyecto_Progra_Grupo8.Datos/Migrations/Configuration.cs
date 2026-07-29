namespace Proyecto_Progra_Grupo8.Datos.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Proyecto_Progra_Grupo8.Entidades.Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Proyecto_Progra_Grupo8.Datos.ProyectoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Proyecto_Progra_Grupo8.Datos.ProyectoDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            foreach (var rol in new[] { "Administrador", "Asociado" })
            {
                if (!roleManager.RoleExists(rol))
                    roleManager.Create(new IdentityRole(rol));
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            const string adminEmail = "admin@cooperativa.cr";
            if (userManager.FindByName(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador Cooperativa",
                    Cedula = "000000000",
                    FechaNacimiento = new DateTime(1990, 1, 1),
                    NumeroAsociado = "AS-0001",
                    FechaIngreso = DateTime.Now,
                    EmailConfirmed = true
                };

                if (userManager.Create(admin, "Admin123!").Succeeded)
                    userManager.AddToRole(admin.Id, "Administrador");
            }

            context.SaveChanges();
        }
    }
}

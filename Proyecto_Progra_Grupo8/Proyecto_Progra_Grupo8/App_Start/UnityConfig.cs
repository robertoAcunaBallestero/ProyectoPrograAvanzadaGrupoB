using Proyecto_Progra_Grupo8.Datos;
using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Infraestructura;
using Proyecto_Progra_Grupo8.Negocio.Services;
using Proyecto_Progra_Grupo8.Controllers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace Proyecto_Progra_Grupo8
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; private set; }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Configuración DbContext y repositorios
            container.RegisterType<ProyectoDbContext>(
                new HierarchicalLifetimeManager());

            container.RegisterType
                <IEventoRepository, EventoRepository>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <ICarteleraRepository, CarteleraRepository>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <ICompraRepository, CompraRepository>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <IUsuarioRepository, UsuarioRepository>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <IResenaRepository, ResenaRepository>(
                    new HierarchicalLifetimeManager());

            // Servicios
            container.RegisterType
                <IEventoService, EventoService>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <ICarteleraService, CarteleraService>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <ICompraService, CompraService>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <IUsuarioService, UsuarioService>(
                    new HierarchicalLifetimeManager());

            container.RegisterType
                <IResenaService, ResenaService>(
                    new HierarchicalLifetimeManager());

            // Registro explícito para AccountController usando OWIN / Identity
            container.RegisterType<AccountController>(
                new InjectionConstructor());

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c =>
                    HttpContext.Current
                        .GetOwinContext()
                        .Authentication));

            DependencyResolver.SetResolver(
                new UnityDependencyResolver(container));

            Container = container;

            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityWebApiDependencyResolver(container);
        }
    }
}
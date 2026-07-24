using Proyecto_Progra_Grupo8.Datos;
using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace Proyecto_Progra_Grupo8
{

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();


            container.RegisterType<ProyectoDbContext>(
                new HierarchicalLifetimeManager());


            container.RegisterType
                <IEventoRepository, EventoRepository>(
                    new HierarchicalLifetimeManager());


            container.RegisterType
                <IEventoService, EventoService>(
                    new HierarchicalLifetimeManager());


            DependencyResolver.SetResolver(
                new UnityDependencyResolver(container));
        }
    }
}
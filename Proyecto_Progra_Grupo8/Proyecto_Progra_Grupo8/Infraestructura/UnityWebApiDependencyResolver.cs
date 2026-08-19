using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;

namespace Proyecto_Progra_Grupo8.Infraestructura
{
    // Adapta el UnityContainer ya usado por MVC (UnityConfig) para que
    // ASP.NET Web API también pueda resolver ISomeService en sus controladores.
    public class UnityWebApiDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityWebApiDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new UnityWebApiDependencyResolver(
                _container.CreateChildContainer());
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new object[0];
            }
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}

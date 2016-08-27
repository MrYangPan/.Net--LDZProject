using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using AF.Core.Infrastructure;


namespace AF.Web.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiDependencyResolver : IDependencyResolver
    {
        private IEngine CurrentEngineContext => EngineContext.Current;

        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return CurrentEngineContext.ContainerManager.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)CurrentEngineContext.Resolve(type);
        }

        /// <summary>
        /// Begins the scope.
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}

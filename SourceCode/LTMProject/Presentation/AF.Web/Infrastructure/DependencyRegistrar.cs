using AF.Core.Caching;
using AF.Core.Infrastructure;
using AF.Core.Infrastructure.DependencyManagement;
using AF.Web.Controllers;
using Autofac;
using Autofac.Core;

namespace AF.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            builder.RegisterType<HomeController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("af_cache_static"));
        }

        public int Order
        {
            get { return 2; }
        }
    }
}

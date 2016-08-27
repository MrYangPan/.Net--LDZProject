using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Admin.Controllers;
using AF.Core.Infrastructure;
using AF.Core.Infrastructure.DependencyManagement;
using Autofac;
using Autofac.Core;
using WebGrease;

namespace AF.Admin.Infrastructure
{
    public class DependencyRegistrar: IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            //builder.RegisterType<HomeController>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("af_cache_static"));
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AF.Core;
using AF.Core.Caching;
using AF.Core.Configuration;
using AF.Core.Data;
using AF.Core.Fakes;
using AF.Core.Infrastructure;
using AF.Core.Infrastructure.DependencyManagement;
using AF.Data.DbContext;
using AF.Domain.Infrastructure;
using AF.Services.Authentication;
using AF.Services.Customers;
using AF.Services.Events;
using AF.Services.Knowledge;
using AF.Services.Message;
using AF.Services.Security;
using AF.Services.Tasks;
using AF.Web.Framework;
using Autofac;

namespace AF.Services.Tests
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c => 
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();


            //data layer
            var dataSettingsManager = new DataSettingsManager();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();
            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            var config = Singleton<AFConfig>.Instance;

            var afdefalutcontext = new AFObjectContext(config.AfDataConnetcion);
            afdefalutcontext.Database.Log = s => Debug.Print(s);
            builder.Register<IDbContext>(c => afdefalutcontext).Named<IDbContext>("AfDataConnetcion").InstancePerLifetimeScope();

            var defalutcontext = new AFObjectContext(config.DefalutConnetcion);
            defalutcontext.Database.Log = s => Debug.Print(s);
            builder.Register<IDbContext>(c => defalutcontext).InstancePerLifetimeScope();
            Database.SetInitializer<AFObjectContext>(null);




            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
    
            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("af_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("af_cache_per_request").InstancePerLifetimeScope();

            //work context
    
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();


            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();

            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();

            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
        
            builder.RegisterType<MessageServices>().As<IMessageServices>().InstancePerLifetimeScope();

            builder.RegisterType<KnowledgeService>().As<IKnowledgeService>().InstancePerLifetimeScope();
  


            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        public int Order
        {
            get { return 0; }
        }
    }


  
}

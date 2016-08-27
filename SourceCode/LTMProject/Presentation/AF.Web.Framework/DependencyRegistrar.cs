using System;
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
using AF.Services.Books;
using AF.Services.BookWork;
using AF.Services.Common;
using AF.Services.Customers;
using AF.Services.Events;
using AF.Services.Logging;
using AF.Services.Security;
using AF.Services.Tasks;
using AF.Services.Knowledge;
using AF.Services.Message;
using AF.Services.Question;
using AF.Services.TiMus;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace AF.Web.Framework
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

            //web api controllers
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());
            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
         
            //data layer
            var dataSettingsManager = new DataSettingsManager();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();
            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            var config = Singleton<AFConfig>.Instance;
            builder.Register<IDbContext>(c => new AFObjectContext(config.AfDataConnetcion)).Named<IDbContext>("AfDataConnetcion").InstancePerLifetimeScope();
            builder.Register<IDbContext>(c => new AFObjectContext(config.DefalutConnetcion)).InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("af_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("af_cache_per_request").InstancePerLifetimeScope();


            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();

            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();

            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();

            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageServices>().As<IMessageServices>().InstancePerLifetimeScope();
            builder.RegisterType<KnowledgeService>().As<IKnowledgeService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionService>().As<IQuestionService>().InstancePerLifetimeScope();
            builder.RegisterType<SubjectService>().As<ISubjectService>().InstancePerLifetimeScope();
            builder.RegisterType<TiMuService>().As<ITiMuService>().InstancePerLifetimeScope();
            builder.RegisterType<BookWorkTaskService>().As<IBookWorkTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<BookChapterService>().As<IBookChapterService>().InstancePerLifetimeScope();
            builder.RegisterType<BookService>().As<IBookService>().InstancePerLifetimeScope();
            builder.RegisterType<BookWorkTaskItemService>().As<IBookWorkTaskItemService>().InstancePerLifetimeScope();

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

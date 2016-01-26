using Autofac;
using Autofac.Integration.Mvc;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace LANBackup.AdminPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        IBus bus;
        protected void Application_Start()
        {
            ConfigureServiceBus();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();           
        }
        public override void Dispose()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
            base.Dispose();
        }
        private void ConfigureServiceBus()
        {           
            ContainerBuilder builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(new AutofacWebApiDependencyResolver(container));
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.Mvc.WebApplication");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            var startableBus = Bus.Create(busConfiguration);
            bus = startableBus.Start();
        }
    }
}

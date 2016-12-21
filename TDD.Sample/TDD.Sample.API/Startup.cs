using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using TDD.Sample.API.Core;
using TDD.Sample.API.Core.Controllers;
using TDD.Sample.API.Core.MediaTypeFormatters;
using TDD.Sample.Data;
using TDD.Sample.Services;

[assembly: OwinStartup(typeof(TDD.Sample.API.Startup))]
namespace TDD.Sample.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());
            config.Formatters.Add(new ArticleFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(BlogsController).Assembly);
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            //Repositories
            builder.RegisterAssemblyTypes(typeof(BlogRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            // Services
            builder.RegisterAssemblyTypes(typeof(ArticleService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}

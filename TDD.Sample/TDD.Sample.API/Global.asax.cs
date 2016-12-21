using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TDD.Sample.Data;

namespace TDD.Sample.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            // Init database
            System.Data.Entity.Database.SetInitializer(new BloggerInitializer());
        }
    }
}

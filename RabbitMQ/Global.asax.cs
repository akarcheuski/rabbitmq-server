using RabbitMQ.App_Start;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RabbitMQ
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IoCConfig.RegisterDependencies();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.Session["MessagesQuantity"] = 0;
            System.Web.HttpContext.Current.Session["Time"] = 0;
        }
    }
}

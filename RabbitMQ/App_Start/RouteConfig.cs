using System.Web.Mvc;
using System.Web.Routing;

namespace RabbitMQ
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "SendMessages",
               url: "Default/SendMessages",
               defaults: new { controller = "Default", action = "SendMessages" }
            );
            routes.MapRoute(
             name: "StopSending",
             url: "Default/StopSending",
             defaults: new { controller = "Default", action = "StopSending" }
            );
        }     
    }
}

using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(EventSourcing.Web.MvcConfig), "Configure")]
namespace EventSourcing.Web
{
    public class MvcConfig
    {
        public static void Configure()
        {
            RouteTable.Routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional});

        }
         
    }
}
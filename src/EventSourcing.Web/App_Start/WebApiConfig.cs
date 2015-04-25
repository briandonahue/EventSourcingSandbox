using System.Web.Http;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(EventSourcing.Web.WebApiConfig), "Configure")]
namespace EventSourcing.Web
{
    public class WebApiConfig
    {
        public static void Configure()
        {
            GlobalConfiguration.Configure(c =>
            {
                c.MapHttpAttributeRoutes();
                c.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                    );
            });
        }
         
    }
}
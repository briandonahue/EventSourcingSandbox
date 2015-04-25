using System.Web.Mvc;

namespace EventSourcing.Web.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
         
    }
}
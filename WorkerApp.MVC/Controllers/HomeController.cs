using System.Web.Mvc;

namespace WorkerApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Menu() 
        {
            return PartialView("_NavBarHeader");
        }
    }
}
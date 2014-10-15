using System.Web.Mvc;
using SqlSaturdayCode.Models;

namespace SqlSaturdayCode.Controllers
{
    public class HomeController : RavenBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Speaker(int id)
        {
            var speaker = DocumentSession.Load<Speaker>(id);
            return View(speaker);
        }
    }
}
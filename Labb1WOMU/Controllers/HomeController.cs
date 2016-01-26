using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labb1WOMU.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            ViewBag.Message = "Här kanske vi kan ha våra produkter";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt information:";

            return View();
        }
    }
}
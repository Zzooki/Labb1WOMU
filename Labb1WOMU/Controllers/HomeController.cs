using Labb1WOMU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Labb1WOMU.Controllers
{
    public class HomeController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            ViewBag.Message = "Här kanske vi kan ha våra produkter";
            return View();
        }

        public ActionResult Search(int query)
        {
            var result = from order in db.Order
                         select order;

            if (query > 0)
            {
                result = result.Where(s => s.OrderId == query);
            }
            return View(result);

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt information:";

            return View();
        }
    }
}
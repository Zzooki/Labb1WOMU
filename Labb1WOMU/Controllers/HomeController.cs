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

        /// <summary>
        /// Denna metod används för att visa sidans home vy.
        /// </summary>
        /// <returns>
        /// Returnerar home vyn.
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Denna metod används för att visa information på startsidan.
        /// </summary>
        /// <returns>
        /// Returnerar vyn där den information som skall visas, visas.
        /// </returns>
        public ActionResult Products()
        {
            ViewBag.Message = "Här kanske vi kan ha våra produkter";
            return View();
        }

        /// <summary>
        /// Länk till kontakt information.
        /// </summary>
        /// <returns>
        /// Returnerar vyn där kontakt informationen visas.
        /// </returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt information:";

            return View();
        }
    }
}
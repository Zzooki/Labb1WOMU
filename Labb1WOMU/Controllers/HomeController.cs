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
        /// Denna metod ansvarar för att returnera vyn för första sidan
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Denna metod ansvarar för att returnera vyn där vi kan hitta vår kontakt information
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt information:";

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labb1WOMU.Models;

namespace Labb1WOMU.Controllers
{
    public class ArtikelController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();

        // GET: Artikel
        /// <summary>
        /// Denna metod returnerar en view med samtliga artiklar i databasen.
        /// </summary>
        /// <returns>
        /// Returnerar en vy med samtliga artiklar.
        /// </returns>
        public ActionResult Index()
        {
            return View(db.Artikel.ToList());
        }

        // GET: Artikel/Details/5
        /// <summary>
        /// Denna metod används för att ta fram en detaljerad vy för en given artikel.
        /// </summary>
        /// <param name="id">
        /// Idt för artikeln som en detaljerad vy skall visas för.
        /// </param>
        /// <returns>
        /// Returnerar den detaljerade vyn för den valda produkten.
        /// </returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artikel artikel = db.Artikel.Find(id);
            if (artikel == null)
            {
                return HttpNotFound();
            }
            return View(artikel);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

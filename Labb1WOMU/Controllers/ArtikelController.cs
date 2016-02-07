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

        // GET: Artikel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artikel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Denna metod används för att lägga till en ny produkt i databasen.
        /// </summary>
        /// <param name="artikel">
        /// Metoden tar in information om en produkt från ett webbformulär och skapar en artikel instans med den
        /// datan som sedan sparas till databasen.</param>
        /// <returns>
        /// Metoden returnerar index vyn för artikel om artikeln lyckades skapas annars får användaren en ny chans att mata
        /// in produkten.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtikelID,ArtikelNamn,Beskrivning,Antal,Pris,Aktuell")] Artikel artikel)
        {
            if (ModelState.IsValid)
            {
                db.Artikel.Add(artikel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artikel);
        }

        // GET: Artikel/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Artikel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Denna metod används för att ändra på informationen om en artikel som finns i databasen.
        /// Detta görs med information som matas in av användare genom ett webbförmulär.
        /// </summary>
        /// <param name="artikel">
        /// En artikel instans skapas med information från ett webbformulär, sedan sparas artikelinstansen till databasen.
        /// </param>
        /// <returns>
        /// Metoden returnerar artikel/index vyn om artikeln ändrades utan fel, och annars får användren en ny chans
        /// att göra ändringarna.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtikelID,ArtikelNamn,Beskrivning,Antal,Pris,Aktuell")] Artikel artikel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artikel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artikel);
        }

        // GET: Artikel/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Artikel/Delete/5
        /// <summary>
        /// Denna metod används för att ta bort en artikel från databasen.
        /// </summary>
        /// <param name="id">
        /// Idt på produkten som skall tas bort från databasen.
        /// </param>
        /// <returns>
        /// Returnerar index metoden för klassen.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artikel artikel = db.Artikel.Find(id);
            db.Artikel.Remove(artikel);
            db.SaveChanges();
            return RedirectToAction("Index");
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

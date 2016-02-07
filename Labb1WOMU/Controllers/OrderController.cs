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
    public class OrderController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();

        // GET: Order
        /// <summary>
        /// Denna metod skapar en vy för att visa alla ordrar som tillhör en given kund.
        /// </summary>
        /// <returns>
        /// Returnerar en vy som visar de ordrar som tillhör kuden.
        /// </returns>
        public ActionResult Index()
        {
            var order = db.Order.Include(o => o.Kund);
            return View(order.ToList());
        }
        
        /// <summary>
        /// Denna metod används för att söka efter ordrar i databasen.
        /// </summary>
        /// <param name="searchValue">
        /// Idt på den order som användaren matat in.
        /// </param>
        /// <returns>
        /// returnerar en vy med den order som användaren matat in, eller en tom vy om ingen order hittats.
        /// </returns>
        public ActionResult Search(string searchValue)
        {
     
            var result = from order in db.Order
                         select order;
            int value;

            if (!String.IsNullOrEmpty(searchValue) && Int32.TryParse(searchValue, out value))
            {
                result = result.Where(s => s.OrderId.Equals(value));
            }
            else
            {
                return new EmptyResult();
            }


            return View(result);

        }



        // GET: Order/Details/5
        /// <summary>
        /// Denna metod används för att skapa en detaljerad vy över en viss order.
        /// </summary>
        /// <param name="id">
        /// Idt på ordern som skall detaljer skall visas om.
        /// </param>
        /// <returns>
        /// Returnerar en detaljerad vy över den order som matats in, eller en vy som indikerar att orden inte
        /// hittats om ingen order hittats.
        /// </returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.KundID = new SelectList(db.Kund, "KundID", "Förnamn");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Denna metod används för att skapa en order.
        /// </summary>
        /// <param name="order">
        /// Metoden tar in information från ett webbformulär och skapar en order instans med den datan, och 
        /// det objektet sparas sedan till databasen.</param>
        /// <returns>
        /// Returnerar en vy med den skapade ordern om det gick att skapa den, om det inte gick
        /// så får användaren en ny chans att fylla i formuläret.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,KundID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KundID = new SelectList(db.Kund, "KundID", "Förnamn", order.KundID);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.KundID = new SelectList(db.Kund, "KundID", "Förnamn", order.KundID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Denna metod används för att ändra på en order som redan finns sparad i databasen.
        /// </summary>
        /// <param name="order">
        /// Tar in information som skall sparas till existerande order från ett webbformulär detta används
        /// för att skapa en order instans som sedan används för att uppdaterade den redan existerande ordern.
        /// </param>
        /// <returns>
        /// Metoden returnerar index metoden i klassen om ändringen lyckades, annars får användaren en ny chans att
        /// fylla i formuläret.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,KundID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KundID = new SelectList(db.Kund, "KundID", "Förnamn", order.KundID);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        /// <summary>
        /// Denna metod används för att ta bort en order som finns i databasen.
        /// </summary>
        /// <param name="id">
        /// Idt på ordern som skall tas bort.
        /// </param>
        /// <returns>
        /// Metoden returnerar klassens index metod.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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

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
    public class OrderradController : Controller
    {
        private DBTEntities db = new DBTEntities();

        // GET: Orderrad
        public ActionResult Index()
        {
            var orderrads = db.Orderrads.Include(o => o.Artikel).Include(o => o.Order);
            return View(orderrads.ToList());
        }

        // GET: Orderrad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderrad orderrad = db.Orderrads.Find(id);
            if (orderrad == null)
            {
                return HttpNotFound();
            }
            return View(orderrad);
        }

        // GET: Orderrad/Create
        public ActionResult Create()
        {
            ViewBag.ArtikelID = new SelectList(db.Artikels, "ArtikelID", "ArtikelNamn");
            ViewBag.OrderID = new SelectList(db.Orders, "OrderId", "OrderId");
            return View();
        }

        // POST: Orderrad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ArtikelID,Antal")] Orderrad orderrad)
        {
            if (ModelState.IsValid)
            {
                db.Orderrads.Add(orderrad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtikelID = new SelectList(db.Artikels, "ArtikelID", "ArtikelNamn", orderrad.ArtikelID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderId", "OrderId", orderrad.OrderID);
            return View(orderrad);
        }

        // GET: Orderrad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderrad orderrad = db.Orderrads.Find(id);
            if (orderrad == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtikelID = new SelectList(db.Artikels, "ArtikelID", "ArtikelNamn", orderrad.ArtikelID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderId", "OrderId", orderrad.OrderID);
            return View(orderrad);
        }

        // POST: Orderrad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ArtikelID,Antal")] Orderrad orderrad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderrad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtikelID = new SelectList(db.Artikels, "ArtikelID", "ArtikelNamn", orderrad.ArtikelID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderId", "OrderId", orderrad.OrderID);
            return View(orderrad);
        }

        // GET: Orderrad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderrad orderrad = db.Orderrads.Find(id);
            if (orderrad == null)
            {
                return HttpNotFound();
            }
            return View(orderrad);
        }

        // POST: Orderrad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orderrad orderrad = db.Orderrads.Find(id);
            db.Orderrads.Remove(orderrad);
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

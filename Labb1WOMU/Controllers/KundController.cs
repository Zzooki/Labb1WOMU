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
    public class KundController : Controller
    {
        private DBTEntities db = new DBTEntities();

        // GET: Kund
        public ActionResult Index()
        {
            var kunds = db.Kunds.Include(k => k.PostNrOrt);
            return View(kunds.ToList());
        }

        // GET: Kund/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kunds.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }

        // GET: Kund/Create
        public ActionResult Create()
        {
            ViewBag.PostNr = new SelectList(db.PostNrOrts, "PostNr", "Ort");
            return View();
        }

        // POST: Kund/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KundID,Förnamn,Efternamn,Postadress,PostNr,Epost,TelefonNr")] Kund kund)
        {
            if (ModelState.IsValid)
            {
                db.Kunds.Add(kund);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostNr = new SelectList(db.PostNrOrts, "PostNr", "Ort", kund.PostNr);
            return View(kund);
        }

        // GET: Kund/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kunds.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostNr = new SelectList(db.PostNrOrts, "PostNr", "Ort", kund.PostNr);
            return View(kund);
        }

        // POST: Kund/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KundID,Förnamn,Efternamn,Postadress,PostNr,Epost,TelefonNr")] Kund kund)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kund).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostNr = new SelectList(db.PostNrOrts, "PostNr", "Ort", kund.PostNr);
            return View(kund);
        }

        // GET: Kund/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kunds.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }

        // POST: Kund/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kund kund = db.Kunds.Find(id);
            db.Kunds.Remove(kund);
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

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
    public class PostNrOrtsController : Controller
    {
        private DBTEntities db = new DBTEntities();

        // GET: PostNrOrts
        public ActionResult Index()
        {
            return View(db.PostNrOrts.ToList());
        }

        // GET: PostNrOrts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostNrOrt postNrOrt = db.PostNrOrts.Find(id);
            if (postNrOrt == null)
            {
                return HttpNotFound();
            }
            return View(postNrOrt);
        }

        // GET: PostNrOrts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostNrOrts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostNr,Ort")] PostNrOrt postNrOrt)
        {
            if (ModelState.IsValid)
            {
                db.PostNrOrts.Add(postNrOrt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postNrOrt);
        }

        // GET: PostNrOrts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostNrOrt postNrOrt = db.PostNrOrts.Find(id);
            if (postNrOrt == null)
            {
                return HttpNotFound();
            }
            return View(postNrOrt);
        }

        // POST: PostNrOrts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostNr,Ort")] PostNrOrt postNrOrt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postNrOrt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postNrOrt);
        }

        // GET: PostNrOrts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostNrOrt postNrOrt = db.PostNrOrts.Find(id);
            if (postNrOrt == null)
            {
                return HttpNotFound();
            }
            return View(postNrOrt);
        }

        // POST: PostNrOrts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostNrOrt postNrOrt = db.PostNrOrts.Find(id);
            db.PostNrOrts.Remove(postNrOrt);
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

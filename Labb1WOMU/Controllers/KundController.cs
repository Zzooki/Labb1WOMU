using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labb1WOMU.Models;
using System.Text.RegularExpressions;
using Labb1WOMU.ViewModels;
using System.Web.UI.WebControls;

namespace Labb1WOMU.Controllers
{
    public class KundController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();

        // GET: Kund
        public ActionResult Index()
        {
            return View(db.Kund.ToList());
        }

        // GET: Kund/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }

        // GET: Kund/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kund/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KundID,Förnamn,Efternamn,Postadress,PostNr,Epost,TelefonNr,Ort")] Kund kund)
        {
            if (!isOnlyLetters(kund.Förnamn) || !isOnlyLetters(kund.Efternamn))
            {
                //var namnView = new KundCreateViewModel()
                //{
                //    Message = "Förnamn/Efternamn får endast innehålla bökstäver!"
                //};
                return PartialView("KundCreateView", kund);
            }

            if (!IsValidEmail(kund.Epost))
            {
                var emailView = new KundCreateViewModel
                {
                    Message = "Epost måste vara i korrekt format: asd@domain.com"
                };
                return PartialView("KundCreateView", kund);
            }
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                var items = cart.GetCartItems();

            if (ModelState.IsValid)
            {
                    foreach (var item in items)
                    {
                        var produkt = db.Artikel.Single(
                        artikel => artikel.ArtikelID == item.ArtikelID);
                        if (item.Count > produkt.Antal)
                        {
                            return View(kund);
                        }
                    }

                db.Kund.Add(kund);
                db.SaveChanges();
                var searchTemp = from sc in db.Kund select sc;

                var kundTemp = searchTemp.Where(f => f.Förnamn.Equals(kund.Förnamn) && f.Efternamn.Equals(kund.Efternamn) && f.Postadress.Equals(kund.Postadress) && f.PostNr.Equals(kund.PostNr) && f.Epost.Equals(kund.Epost) && f.Ort.Equals(kund.Ort));
                var kundanother = kundTemp.Single();
                order.KundID = kundanother.KundID;
                db.Order.Add(order);
                db.SaveChanges();

                    cart.CreateOrder(order);
                ViewData["OrderID"] = order.OrderId;

                    return RedirectToAction("ConfirmOrder", new { id = order.OrderId });
                }
            }
            catch
            {
                return View(kund);
            }
            return View(kund);
            }

            bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool isOnlyLetters(string  s)
        {
            return Regex.IsMatch(s, @"^[a-öA-Ö]+$");
        }

        // GET: Kund/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }

        // POST: Kund/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KundID,Förnamn,Efternamn,Postadress,PostNr,Epost,TelefonNr,Ort")] Kund kund)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kund).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kund);
        }

        // GET: Kund/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
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
            Kund kund = db.Kund.Find(id);
            db.Kund.Remove(kund);
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
        public ActionResult ConfirmOrder(int id)
        {
            bool isValid = db.Order.Any(
            o => o.OrderId == id);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}

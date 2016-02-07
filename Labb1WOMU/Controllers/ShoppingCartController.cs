using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labb1WOMU.Models;
using System.Web.UI.WebControls;
using Labb1WOMU.ViewModels;

namespace Labb1WOMU.Controllers
{
    public class ShoppingCartController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();

        // GET: Cart
        /// <summary>
        /// Denna metod skapar index viewen för varukorgen.
        /// </summary>
        /// <returns></returns>
        /// Returnerar en view för varukorgens index.
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var recommendedItems = GetRecommendedItems(cart);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                RecommendedItems = recommendedItems
            };

            return View(viewModel);
        }
        /// <summary>
        /// Denna metod skapar en lista med rekommenderade artiklar baserat på vad andra användare har köpt i samband
        /// med de artiklar som finns i den aktuella varukorgen.
        /// </summary>
        /// <param name="cart"></param>
        /// Den aktuella varukorgen.
        /// <returns></returns>
        /// En lista med rekommenderade artiklar.
        public List<Artikel> GetRecommendedItems(ShoppingCart cart)
        {
            List<Artikel> recommendedItems = new List<Artikel>();
            var orders = db.Orderrad.ToList();
            var items = cart.GetCartItems();
            foreach (var item in items)
            {
                foreach (var order in orders)
                {
                    if (item.ArtikelID != order.ArtikelID)
                    {
                        recommendedItems.Add(db.Artikel.Find((order.ArtikelID)));
                    }
                }
            }
            return recommendedItems;
        }

        // GET: Cart/Details/5
        /// <summary>
        /// Skapar den detaljerade viewen för varukorgen.
        /// </summary>
        /// <param name="id"></param>
        /// Varukorgens id accepterar null ifall det inte skulle finnas en varukorg.
        /// <returns></returns>
        /// Returnerar viewen för varukorgens detail sida.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        /// <summary>
        /// Denna metod skapar en varukorg.
        /// </summary>
        /// <returns></returns>
        /// Returnerar varukorgen som skapades.
        public ActionResult Create()
        {
            ViewBag.ArtikelID = new SelectList(db.Artikel, "ArtikelID", "ArtikelNamn");
            ViewBag.Date = DateTime.Today;
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartID,ArtikelID,Count,DateCreated")] Cart cart)
        {
            cart.DateCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Cart.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtikelID = new SelectList(db.Artikel, "ArtikelID", "ArtikelNamn", cart.ArtikelID);
            return View(cart);
        }

        // GET: Cart/Edit/5
        /// <summary>
        /// Denna metod skapar en view för att kunna ändra i en kundvagn.
        /// </summary>
        /// <param name="id"></param>
        /// Kundvagnens id, accepterar null ifall att det inte skulle finnas en instans av varukorgen i den
        /// aktuelle sessionen.
        /// <returns></returns>
        /// Returnerar viewen för att ändra i varukorgen.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtikelID = new SelectList(db.Artikel, "ArtikelID", "ArtikelNamn", cart.ArtikelID);
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartID,ArtikelID,Count,DateCreated")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtikelID = new SelectList(db.Artikel, "ArtikelID", "ArtikelNamn", cart.ArtikelID);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        /// <summary>
        /// Denna metod används för att ta bort artiklar från kundvagnen, om det finns mer än en av en artikel i
        /// kundvagnen så blir antalet = antalet - 1, och om det bara finns en av en artikel så tas den bort
        /// helt från varukorgen.
        /// </summary>
        /// <param name="id"></param>
        /// Varukorgens id.
        /// <returns></returns>
        /// Returnerar den uppdaterade varukorgen i Json format.
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string artikelNamn = db.Artikel.Single(item => item.ArtikelID == id).ArtikelNamn;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(artikelNamn) + " har blivit borttagen från din varukorg!",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id,
            };

            return Json(results);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Cart.Find(id);
            db.Cart.Remove(cart);
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
        /// <summary>
        /// Denna metod används för att lägga till artiklar i varukorgen, om artikeln redan finns i varukorgen
        /// plussas antalet på med ett, annars läggs artikeln till i varukorgen med antal 1.
        /// </summary>
        /// <param name="id"></param>
        /// Varukorgens id.
        /// <returns></returns>
        /// Returnerar index metoden som då visar den uppdaterade varukorgen.
        public ActionResult AddToCart(int id)
        {
            var produkt = db.Artikel.Single(
                artikel => artikel.ArtikelID == id);

            var varukorg = ShoppingCart.GetCart(this.HttpContext);

            varukorg.AddToCart(produkt);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Denna metod visar en summering av varukorgen som innehåller de artiklar som finns i varukorgen,
        /// och hur mycket hela varukorgen skulle kosta.
        /// </summary>
        /// <returns></returns>
        /// Returnerar summering som en partiell vy som sedan visas som en tabell på samtliga sidor.
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return PartialView(viewModel);
        }
 
    }
}

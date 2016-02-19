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
        
        // GET: Kund/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kund/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Denna metod skapar en kund utifrån data som kommer ifrån ett formulär som finns på sidan och används när en
        /// order skall slutföras.
        /// </summary>
        /// <param name="kund"></param>
        /// Informationen från webbformuläret används för att skapa ett kund objekt, som sedan valideras och sparas i databasen.
        /// <returns></returns>
        /// Returnerar en vy som visar relevant information.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KundID,Förnamn,Efternamn,Postadress,PostNr,Epost,TelefonNr,Ort")] Kund kund)
        {
            if (kund.error.Message == null)
            {
                kund.error.Message = "";
            }

            if (!isOnlyLetters(kund.Förnamn) || !isOnlyLetters(kund.Efternamn))
            {
                kund.error.Message = "Ditt namn och förnamn får endast innehålla bokstäver!";
                return View(kund);
            }

            if (!IsValidEmail(kund.Epost))
            {
                kund.error.Message = "Epost måste vara i korrekt format: asd@domain.com";
                return View(kund);
            }
            var order = new Order();
            TryUpdateModel(kund.error);

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

        public ActionResult KundCreateView(Kund kund)
        {
            

            return View(kund);
        }

        /// <summary>
        /// Denna metoden ansvarar för att kontrollera så att kunden har angett en valid e-postadress
        /// </summary>
        /// <param name="email"> tar in en sträng som motsvarar kundens e-postadress</param>
        /// <returns>returnerar sant eller falskt om kunden har angett en e-postadress som motsvarar det format som krävs utav databasen</returns>
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
        /// <summary>
        /// Denna metod används för att kolla om en sträng endast innehåller bokstäver.
        /// </summary>
        /// <param name="s">
        /// Strängen som skall valideras.
        /// </param>
        /// <returns>
        /// Returnerar sant om strängen endast innehåller bokstäver, och annars falskt.
        /// </returns>
        public bool isOnlyLetters(string  s)
        {
            return Regex.IsMatch(s, @"^[a-öA-Ö]+$");
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
        /// Denna metod används för att validera en order innan den slutförs.
        /// </summary>
        /// <param name="id">
        /// Ordern som skall valideras id.
        /// </param>
        /// <returns>
        /// Returnerar en vy som antingen visar att orden har slutförts eller ett felmeddelande.
        /// </returns>
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

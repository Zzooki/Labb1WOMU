using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb1WOMU.Models
{
    /// <summary>
    /// Denna klassen inneh�ller alla metoder f�r att hantera inneh�llet i varukorgen
    /// </summary>
    public partial class ShoppingCart
    {
        private DBTEntities1 db = new DBTEntities1();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        
        /// <summary>
        /// Denna klass anv�nds f�r att f� den aktuella kundens varukorg genom att skapa varukorg inneh�llande kundens id.
        /// </summary>
        /// <param name="context">Tar in context i form  utav en HttpContextBase som �r en unik str�ng f�r 
        /// att identifiera kunden/varukorgen </param>
        /// <returns>returnerar en varukorg som �r ett ShoppingCart objekt</returns>
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        /// <summary>
        /// Metoden hanterar borttagning utav varor ut varukorgen om det inte finns n�gra produkter i varukorgen
        /// s� sker ingen borttagning.
        /// </summary>
        /// <param name="id">Tar in ett id i form utav en int som ska motsvara det produkt id som �nskas att
        /// tas bort ur varukorgen</param>
        /// <returns>returnerar antalet produkter i varukorgen efter borttagningen</returns>
        public int RemoveFromCart(int id)
        {
            var cartItem = db.Cart.Single(
                cart => cart.StringCartID == ShoppingCartId && cart.ArtikelID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Cart.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }
        /// <summary>
        /// GetCartId metoden anv�nds f�r att f� det ID som accossieras med den nuvarande kundens
        /// varukorg. Om det inte finns en Session f�r den aktuella kunden s� skapas en ny s�dan 
        /// </summary>
        /// <param name="context">Metoden tar in context i form utav ett HttpContextBase objekt</param>
        /// <returns>Returnerar en st�ng som motsvarar sessions nyckeln f�r varukorgen</returns>
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        /// <summary>
        /// Metoden som hanterar n�r kunden �nskar att l�gga till en produkt i varukorgen
        /// </summary>
        /// <param name="artikel">Tar in ett objekt som motsvarar en artikel</param>
        public void AddToCart(Artikel artikel)
        {
            var shoppingCart = db.Cart.SingleOrDefault(
                cart => cart.StringCartID == ShoppingCartId && cart.ArtikelID == artikel.ArtikelID);

            var produkt = db.Artikel.Single(
            art => art.ArtikelID == artikel.ArtikelID);

            ///Om det inte finns en cart f�r kunden redan eller produkten inte finns vi
            if (shoppingCart == null)
            {
                ///S� skapar vi en ny cart d�r vi l�gger in den artikel som �nskas i varukorgen
                shoppingCart = new Cart
                {
                    ArtikelID = artikel.ArtikelID,
                    StringCartID = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                ///L�gger in varukorgen i databasen
                    db.Cart.Add(shoppingCart);
            }
            ///Om varukorgen finns och produkten redan finns i varukorgen s� �kar vi antalet produkter 
            else
            {
                shoppingCart.Count++;
            }
            ///Sparar �ndringarna i databasen
            if (!(shoppingCart.Count > produkt.Antal))
            {
                db.SaveChanges();
            }
        }
        /// <summary>
        /// EmptyCart hanterar t�mningen utav kundkorgen n�r kunden har handlat klart och 
        /// checkar ut sin varukorg
        /// </summary>
        public void EmptyCart()
        {
            ///Kollar vilken id som motsvarar varukorgen
            var cartItems = db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId);
            ///F�r alla items i varukorgen s� plockar vi bort dem ett och ett
            foreach (var cartItem in cartItems)
            {
                db.Cart.Remove(cartItem);
            }
            ///Spara �ndringarna i databasen
            db.SaveChanges();
        }
        /// <summary>
        /// GetCartItems metoden ansvarar f�r att returnera de produkter som finns i varukorgen
        /// </summary>
        /// <returns>Returnerar ett List objekt som motsvarar varukorgen och kommer d� att inneh�lla alla produkter</returns>
        public List<Cart> GetCartItems()
        {
            return db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId).ToList();
        }

        /// <summary>
        /// GetCount metoden ansvarar f�r att r�kna ut antalet produkter som finns i varukorgen
        /// </summary>
        /// <returns>returnerar en int som motsvarar antalet produkter som just nu finns i varukorgen</returns>
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Cart
                          where cartItems.StringCartID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        /// <summary>
        /// GetTotal metoden ansvarar f�r att r�kna ut summan utav alla de produkter som just 
        /// nu finns i varukorgen inklusive moms
        /// </summary>
        /// <returns>returnerar en decimal som motsvarar total priset inklusive moms</returns>
        public decimal GetTotal()
        {
            ///Multiplicerar antalet utav varje produkt med varje produkts pris och summera detta
            decimal? total = (from cartItems in db.Cart
                              where cartItems.StringCartID == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Artikel.Pris).Sum();
            ///Kolla s� att totalen inte �r null
            if(total != null)
            {
                ///L�gg p� momsen
                decimal tax = 1.25m;
                total = Decimal.Multiply((decimal)total, tax);
            }
            ///Returnera totalen
            return total ?? decimal.Zero;
        }
        /// <summary>
        /// CreateOrder ansvarar f�r att skapa en order i databasen d� kunden �nskar 
        /// att checka ut sin varukorg
        /// </summary>
        /// <param name="order">Tar in ett Order objekt som motsvarar kundens order</param>
        /// <returns>Returnverar en int som motsvarar kundens orderid</returns>
        public int CreateOrder(Order order)
        {
       
            ///H�mta alla items ur varukorgen
            var cartItems = GetCartItems();

            /// F�r varje produkt som finns i varukorgen ska vi skapa en orderrad i datbasen
            foreach (var item in cartItems)
            {
                var orderrad = new Orderrad
                {
                    ArtikelID = item.ArtikelID,
                    OrderID = order.OrderId,
                    Antal = item.Count
                };
               
                db.Orderrad.Add(orderrad);

            }

            ///Spara �ndringarna i databasen
            db.SaveChanges();

            ///Anropa funktionen som ansvarar f�r att t�mma varukorgen
            EmptyCart();

            ///Returnera kundens order id f�r att informera kunden om detta
            return order.OrderId;
        }
    }
}
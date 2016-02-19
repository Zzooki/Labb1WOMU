using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb1WOMU.Models
{
    /// <summary>
    /// Denna klassen innehåller alla metoder för att hantera innehållet i varukorgen
    /// </summary>
    public partial class ShoppingCart
    {
        private DBTEntities1 db = new DBTEntities1();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        
        /// <summary>
        /// Denna klass används för att få den aktuella kundens varukorg genom att skapa varukorg innehållande kundens id.
        /// </summary>
        /// <param name="context">Tar in context i form  utav en HttpContextBase som är en unik sträng för 
        /// att identifiera kunden/varukorgen </param>
        /// <returns>returnerar en varukorg som är ett ShoppingCart objekt</returns>
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        /// <summary>
        /// Metoden hanterar borttagning utav varor ut varukorgen om det inte finns några produkter i varukorgen
        /// så sker ingen borttagning.
        /// </summary>
        /// <param name="id">Tar in ett id i form utav en int som ska motsvara det produkt id som önskas att
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
        /// GetCartId metoden används för att få det ID som accossieras med den nuvarande kundens
        /// varukorg. Om det inte finns en Session för den aktuella kunden så skapas en ny sådan 
        /// </summary>
        /// <param name="context">Metoden tar in context i form utav ett HttpContextBase objekt</param>
        /// <returns>Returnerar en stäng som motsvarar sessions nyckeln för varukorgen</returns>
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
        /// Metoden som hanterar när kunden önskar att lägga till en produkt i varukorgen
        /// </summary>
        /// <param name="artikel">Tar in ett objekt som motsvarar en artikel</param>
        public void AddToCart(Artikel artikel)
        {
            var shoppingCart = db.Cart.SingleOrDefault(
                cart => cart.StringCartID == ShoppingCartId && cart.ArtikelID == artikel.ArtikelID);

            var produkt = db.Artikel.Single(
            art => art.ArtikelID == artikel.ArtikelID);

            ///Om det inte finns en cart för kunden redan eller produkten inte finns vi
            if (shoppingCart == null)
            {
                ///Så skapar vi en ny cart där vi lägger in den artikel som önskas i varukorgen
                shoppingCart = new Cart
                {
                    ArtikelID = artikel.ArtikelID,
                    StringCartID = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                ///Lägger in varukorgen i databasen
                    db.Cart.Add(shoppingCart);
            }
            ///Om varukorgen finns och produkten redan finns i varukorgen så ökar vi antalet produkter 
            else
            {
                shoppingCart.Count++;
            }
            ///Sparar ändringarna i databasen
            if (!(shoppingCart.Count > produkt.Antal))
            {
                db.SaveChanges();
            }
        }
        /// <summary>
        /// EmptyCart hanterar tömningen utav kundkorgen när kunden har handlat klart och 
        /// checkar ut sin varukorg
        /// </summary>
        public void EmptyCart()
        {
            ///Kollar vilken id som motsvarar varukorgen
            var cartItems = db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId);
            ///För alla items i varukorgen så plockar vi bort dem ett och ett
            foreach (var cartItem in cartItems)
            {
                db.Cart.Remove(cartItem);
            }
            ///Spara ändringarna i databasen
            db.SaveChanges();
        }
        /// <summary>
        /// GetCartItems metoden ansvarar för att returnera de produkter som finns i varukorgen
        /// </summary>
        /// <returns>Returnerar ett List objekt som motsvarar varukorgen och kommer då att innehålla alla produkter</returns>
        public List<Cart> GetCartItems()
        {
            return db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId).ToList();
        }

        /// <summary>
        /// GetCount metoden ansvarar för att räkna ut antalet produkter som finns i varukorgen
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
        /// GetTotal metoden ansvarar för att räkna ut summan utav alla de produkter som just 
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
            ///Kolla så att totalen inte är null
            if(total != null)
            {
                ///Lägg på momsen
                decimal tax = 1.25m;
                total = Decimal.Multiply((decimal)total, tax);
            }
            ///Returnera totalen
            return total ?? decimal.Zero;
        }
        /// <summary>
        /// CreateOrder ansvarar för att skapa en order i databasen då kunden önskar 
        /// att checka ut sin varukorg
        /// </summary>
        /// <param name="order">Tar in ett Order objekt som motsvarar kundens order</param>
        /// <returns>Returnverar en int som motsvarar kundens orderid</returns>
        public int CreateOrder(Order order)
        {
       
            ///Hämta alla items ur varukorgen
            var cartItems = GetCartItems();

            /// För varje produkt som finns i varukorgen ska vi skapa en orderrad i datbasen
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

            ///Spara ändringarna i databasen
            db.SaveChanges();

            ///Anropa funktionen som ansvarar för att tömma varukorgen
            EmptyCart();

            ///Returnera kundens order id för att informera kunden om detta
            return order.OrderId;
        }
    }
}
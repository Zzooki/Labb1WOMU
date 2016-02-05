using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb1WOMU.Models
{
    public partial class ShoppingCart
    {
        private DBTEntities1 db = new DBTEntities1();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

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
        public void AddToCart(Artikel artikel)
        {
            var produkt = db.Cart.SingleOrDefault(
                cart => cart.StringCartID == ShoppingCartId && cart.ArtikelID == artikel.ArtikelID);

            if (produkt == null)
            {
                produkt = new Cart
                {
                    ArtikelID = artikel.ArtikelID,
                    StringCartID = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Cart.Add(produkt);
            }
            else
            {
                produkt.Count++;
            }
            db.SaveChanges();
        }
        public void EmptyCart()
        {
            var cartItems = db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Cart.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return db.Cart.Where(
                cart => cart.StringCartID == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Cart
                          where cartItems.StringCartID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in db.Cart
                              where cartItems.StringCartID == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Artikel.Pris).Sum();
            if(total != null)
            {
                decimal tax = 1.25m;
                total = Decimal.Multiply((decimal)total, tax);
            }

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order, int kundId)
        {
       

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
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

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
    }
}
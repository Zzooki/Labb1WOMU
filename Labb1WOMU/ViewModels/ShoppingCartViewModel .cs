using Labb1WOMU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb1WOMU.ViewModels
{
    /// <summary>
    /// Denna klass hanterar all information som rör varukorgen, exempelvis de varor som just nu 
    /// lagras i varukorgen. Vad summan för alla produkter i varukorgen är, inklusive moms och hur
    /// många produkter det är. 
    /// Den hanterar även information kring listan över de rekommenderade produkter som visas för kund
    /// basserat på de nuvarande innehållet i kundvagnen.
    /// </summary>
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public List<Artikel> RecommendedItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal ProductTotal { get; set; }
    }
}
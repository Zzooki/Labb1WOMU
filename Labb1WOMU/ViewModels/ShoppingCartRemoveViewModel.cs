using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb1WOMU.ViewModels
{
    /// <summary>
    /// Denna klass hanterar informationen som rör borttagning utav varor i varukorgen,
    /// bland annat meddelandet som ska visas för kund, hur många varor som finns kvar och
    /// vilket id på artikeln som har blivit borttagen
    /// </summary>
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}
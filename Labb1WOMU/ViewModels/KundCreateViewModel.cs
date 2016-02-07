using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb1WOMU.ViewModels
{
    /// Denna viewmodell hanterar informationen för att skicka meddelande till kunden angående 
    /// för felinmatningar i registreringsprocessen i samband med utcheckningen utav varukorgen
    public class KundCreateViewModel
    {
        public string Message { get; set; }
    }
}
﻿using Labb1WOMU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb1WOMU.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int ShoppingCartRandomId { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}
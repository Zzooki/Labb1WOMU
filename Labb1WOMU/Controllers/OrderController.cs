using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labb1WOMU.Models;

namespace Labb1WOMU.Controllers
{
    public class OrderController : Controller
    {
        private DBTEntities1 db = new DBTEntities1();

       
        
        public ActionResult Search(string searchValue)
        {
     
            var result = from order in db.Order
                         select order;
            int value;

            if (!String.IsNullOrEmpty(searchValue) && Int32.TryParse(searchValue, out value))
            {
                result = result.Where(s => s.OrderId.Equals(value));
            }
            else
            {
                return new EmptyResult();
            }


            return View(result);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Labb1WOMU.Models;

namespace Labb1WOMU.Controllers
{
    public class ManageController : Controller
    {
        public ActionResult Betala()
        {
            return View();
        }
    }
    
}
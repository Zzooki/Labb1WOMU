using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Labb1WOMU.Models;

namespace Labb1WOMU.Controllers
{
    public class VarukorgController : Controller
    {
        public ActionResult Betala()
        {
            return View();
        }
        public ActionResult Varor()
        {
            return View();
        }
    }

    

}
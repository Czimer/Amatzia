using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User CurrUser)
        {
            if (ModelState.IsValid)
            {

                // TODO: Validate user and password

                return RedirectToAction("Index", "Home");
            }

            return View(CurrUser);
        }
    }
}
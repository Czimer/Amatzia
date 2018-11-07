using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class LoginController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/[CurrUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User CurrUser)
        {
            if (ModelState.IsValid)
            {
                User EnterUser = AmatziaDB.Users.Where(user => user.UserName == CurrUser.UserName &&
                                                               user.Password == CurrUser.Password).FirstOrDefault();

                // Validate user and password
                if (EnterUser != null)
                {
                    TempData["IsManager"] = EnterUser.IsManager ? 1 : 0;

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Message = "User name or password are incorrect";

            return View(CurrUser);
        }
    }
}
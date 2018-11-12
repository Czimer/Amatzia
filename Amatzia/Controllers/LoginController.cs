using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;
using Amatzia.Utils;

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
            try
            {
                if (ModelState.IsValid)
                {
                    User EnterUser = AmatziaDB.Users.Where(user => user.UserName == CurrUser.UserName &&
                                                                   user.Password == CurrUser.Password).FirstOrDefault();

                    // Validate user and password
                    if (EnterUser != null)
                    {
                        // Set user is manager or not
                        GlobalVars.IsManager = EnterUser.IsManager ? true : false;
                        TempData["IsManager"] = GlobalVars.IsManager.ToString();
                        Session["LoggedUser"] = EnterUser;

                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Message = "User name or password are incorrect";

                return View(CurrUser);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
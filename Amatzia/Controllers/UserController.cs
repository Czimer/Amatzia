using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Gender,DateOfBirth,Country,UserName,Password")] User NewUser)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add user to DB 

                return RedirectToAction("Index", "Home");
            }

            return View(NewUser);
        }
    }
}
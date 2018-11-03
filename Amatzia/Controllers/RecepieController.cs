using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class RecepieController : Controller
    {
        // GET: Recepie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Recepie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User,Name,Ingredients,Instructions,Difficulty,uploadDate")] Recepie NewRecepie)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add recepie to DB 
                return RedirectToAction("Index", "Home");
            }

            return View(NewRecepie);
        }
    }
}
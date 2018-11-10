using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class RecepieController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        public ActionResult Index()
        {
            ViewBag.Selected = "Recepies";

            IEnumerable<Recepie> Recepies = (IEnumerable<Recepie>)TempData["Recepies"] ?? AmatziaDB.Recepies.ToList();

            return View(Recepies);
        }

        // POST: Recepie/Edit/[id]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, UserId, Name, Ingredients, Instructions, Difficulty, UploadDate, duration")] Recepie currRecepie)
        {
            if (ModelState.IsValid)
            {
                AmatziaDB.Entry(currRecepie).State = EntityState.Modified;
                AmatziaDB.SaveChanges();
                return RedirectToAction("Index"); 
            }

            return View(currRecepie);
        }

        public ActionResult GetRecepie()
        {
            ViewBag.Message = "Your recepie page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Name,Ingredients,Instructions,Difficulty,UploadDate,duration")] Recepie NewRecepie)
        {
            if (ModelState.IsValid)
            {
                AmatziaDB.Recepies.Add(NewRecepie);
                AmatziaDB.SaveChanges();

                return RedirectToAction("Index", "Home");
                
            }

            return View(NewRecepie);
        }

        // GET: Recepie/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Recepie/Details/5
        public ActionResult Details(int? RecepieId)
        {
            if (RecepieId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return (this.GetRecepieById(RecepieId));
        }


        [HttpGet]
        public ActionResult Search(
           DateTime? UploadDate,
           string Name,
           string Ingredients,
           int Difficulty,
           int duration)
        {
            IEnumerable<Recepie> FilteredRecepies = AmatziaDB.Recepies.ToList();

            if (UploadDate != null)
            {
                FilteredRecepies = FilteredRecepies.Where(recepie => recepie.UploadDate == UploadDate);
            }


            if (!string.IsNullOrEmpty(Name))
            {
                FilteredRecepies = FilteredRecepies.Where(recepie => recepie.Name.ToUpper().Contains(Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(Ingredients))
            {
                FilteredRecepies = FilteredRecepies.Where(recepie => recepie.Ingredients.ToUpper().Contains(Ingredients.ToUpper()));
            }

            if (Difficulty != null)
            {
                FilteredRecepies = FilteredRecepies.Where(recepie => recepie.Difficulty == Difficulty);
            }

            if (duration != null)
            {
                FilteredRecepies = FilteredRecepies.Where(recepie => recepie.duration == duration);
            }

            TempData["Recepies"] = FilteredRecepies;

            return RedirectToAction("Index", "Recepie");
        }


        // GET: Recepie/Edit/[id]
        public ActionResult Edit(int? RecepieId)
        {
            return (this.GetRecepieById(RecepieId));
        }

        // Get the recepie entity by id
        private ActionResult GetRecepieById(int? RecepieId)
        {
            // Check if recepie id is null
            if (RecepieId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the user entity
            Recepie currRecepie = AmatziaDB.Recepies.Find(RecepieId);

            // Check if recepie with this id was found
            if (currRecepie == null)
            {
                return HttpNotFound("Recepie with recepie id " + RecepieId.ToString() + " not found");
            }

            return View(currRecepie);
        }


        // POST: Recepie/Delete/[id]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int RecepieId)
        {
            // Remove the recepie from DB
            AmatziaDB.Recepies.Remove(AmatziaDB.Recepies.Find(RecepieId));
            AmatziaDB.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
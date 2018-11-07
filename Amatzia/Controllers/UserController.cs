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
    public class UserController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        // GET: User
        public ActionResult Index()
        {
            ViewBag.Selected = "User";

            IEnumerable<User> Users = (IEnumerable<User>)TempData["Users"] ?? AmatziaDB.Users.ToList();

            return View(Users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Gender,DateOfBirth,Country,UserName,Password")] User NewUser)
        {
            if (ModelState.IsValid)
            {
                // Check if user name already exist
                if (UserNameAlreadyExist(NewUser.UserName))
                {
                    ViewBag.Message = "User name already exist";
                }
                else
                {
                    AmatziaDB.Users.Add(NewUser);
                    AmatziaDB.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(NewUser);
        }

        // GET: User/Edit/[id]
        public ActionResult Edit(int? UserId)
        {
            return (this.GetUserById(UserId));
        }

        // POST: User/Edit/[id]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId, FirstName, LastName, Gender, DateOfBirth, Country, UserName, Password")] User CurrUser)
        {
            if (ModelState.IsValid)
            {
                // Check if user name already exist
                if (UserNameAlreadyExist(CurrUser.UserName))
                {
                    ViewBag.Message = "User name already exist";
                }
                else
                {
                    AmatziaDB.Entry(CurrUser).State = EntityState.Modified;
                    AmatziaDB.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(CurrUser);
        }


        // GET: User/Delete/[id]
        public ActionResult Delete(int? UserId)
        {
            return (this.GetUserById(UserId));
        }

        // POST: User/Delete/[id]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFinal(int UserId)
        {
            // Remove the user from DB
            AmatziaDB.Users.Remove(AmatziaDB.Users.Find(UserId));
            AmatziaDB.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: User/Details/5
        public ActionResult Details(int? UserId)
        {
            return (this.GetUserById(UserId));
        }



        // Get the user entity by id
        private ActionResult GetUserById(int? UserId)
        {
            // Check if user id is null
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the user entity
            User currUser = AmatziaDB.Users.Find(UserId);

            // Check if user with this id was found
            if (currUser == null)
            {
                return HttpNotFound("User with user id " + UserId.ToString() + " not found");
            }

            return View(currUser);
        }

        private bool UserNameAlreadyExist(string Username)
        {
            return (AmatziaDB.Users.Where(user => user.UserName == Username).Count() == 1);

        }


    }
}
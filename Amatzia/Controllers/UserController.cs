using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;
using Newtonsoft.Json.Linq;
using Amatzia.Utils;

namespace Amatzia.Controllers
{
    public class UserController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();
        public static List<string> AllCountries = GetCountries();
        private static string prevUserName;

        // GET: User
        public ActionResult Index()
        {
            try
            {
                ViewBag.Selected = "User";
                TempData["Countries"] = AllCountries;
                TempData["IsManager"] = GlobalVars.IsManager.ToString();

                IEnumerable<User> Users = (IEnumerable<User>)TempData["UsersFound"] ?? AmatziaDB.Users.ToList();

                return View(Users);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            TempData["Countries"] = AllCountries;
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Gender,DateOfBirth,Country,UserName,Password")] User NewUser)
        {
            try
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
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: User/Edit/[id]
        public ActionResult Edit(int? UserId)
        {
            try
            {
                TempData["Countries"] = AllCountries;
                User CurrUser = this.GetUserById(UserId);
                prevUserName = CurrUser.UserName;

                return View(CurrUser);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: User/Edit/[id]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId, FirstName, LastName, Gender, DateOfBirth, Country, UserName, Password")] User CurrUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if user name already exist
                    if (CurrUser.UserName != prevUserName && UserNameAlreadyExist(CurrUser.UserName))
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
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        // GET: User/Delete/[id]
        public ActionResult Delete(int? UserId)
        {
            try
            {
                return View(this.GetUserById(UserId));
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: User/Delete/[id]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFinal(int UserId)
        {
            try
            {
                // Remove the user from DB
                AmatziaDB.Users.Remove(AmatziaDB.Users.Find(UserId));
                AmatziaDB.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: User/Details/5
        public ActionResult Details(int? UserId)
        {
            try
            {
                return View(this.GetUserById(UserId));
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: User/Search
        [HttpGet]
        public ActionResult Search([Bind(Include = "UserId, FirstName, LastName, Gender, DateOfBirth, Country, UserName, Password")] User SearchedUser)
        {
            try
            {
                IEnumerable<User> UsersFound = AmatziaDB.Users.ToList();

                if (!string.IsNullOrEmpty(SearchedUser.FirstName))
                {
                    UsersFound = UsersFound.Where(user => user.FirstName.ToUpper().Contains(SearchedUser.FirstName.ToUpper()));
                }

                if (!string.IsNullOrEmpty(SearchedUser.LastName))
                {
                    UsersFound = UsersFound.Where(user => user.LastName.ToUpper().Contains(SearchedUser.LastName.ToUpper()));
                }

                if (GlobalVars.IsManager)
                {
                    if (SearchedUser.DateOfBirth != null)
                    {
                        UsersFound = UsersFound.Where(user => user.DateOfBirth == SearchedUser.DateOfBirth);
                    }

                    if (SearchedUser.Gender != null)
                    {
                        UsersFound = UsersFound.Where(user => user.Gender == SearchedUser.Gender);
                    }

                    if (SearchedUser.Country != null)
                    {
                        UsersFound = UsersFound.Where(user => user.Country == SearchedUser.Country);
                    }
                }

                TempData["UsersFound"] = UsersFound;

                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // Get the user entity by id
        private User GetUserById(int? UserId)
        {
            try
            {
                User foundUser = new User();

                // Check if user id is null
                if (UserId != null)
                {
                    foundUser = AmatziaDB.Users.Find(UserId);
                }

                return (foundUser);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool UserNameAlreadyExist(string Username)
        {
            try
            {
                return (AmatziaDB.Users.Where(user => user.UserName == Username).Count() == 1);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private static List<string> GetCountries()
        {
            try
            {
                List<string> lstCountries = new List<string>();

                string URL = "https://restcountries.eu/rest/v2/all";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json; charset=utf-8";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                    JArray jaCountries = JArray.Parse(reader.ReadToEnd());

                    foreach (JObject CountryObject in jaCountries)
                    {
                        lstCountries.Add(CountryObject["name"].ToString());
                    }
                }

                return (lstCountries);
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

    }
}
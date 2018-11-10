using Amatzia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amatzia.Controllers
{
    public class HomeController : Controller
    {
        // delete
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        public ActionResult Index()
        {
            // delete
            User EnterUser = AmatziaDB.Users.Where(user => user.UserName == "yarin").FirstOrDefault();
            Session["LoggedUser"] = EnterUser;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
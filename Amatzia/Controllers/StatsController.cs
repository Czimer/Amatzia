using Amatzia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amatzia.Controllers
{
    public class StatsController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        // GET: Stats
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUsersByCountry()
        {
            var CountriesGroups = AmatziaDB.Users.GroupBy(x => x.Country).Select(group => new
            {
                country = group.Key,
                count = group.Count()
            }).AsEnumerable();

            return Json(CountriesGroups, JsonRequestBehavior.AllowGet);
        }
    }
}
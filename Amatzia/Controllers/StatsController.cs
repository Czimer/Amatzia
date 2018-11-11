using Amatzia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amatzia.Utils;

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

        [HttpGet]
        public ActionResult GetGenderAndPositiveComments()
        {
            List<Recepie> RecRecepies = new List<Recepie>();

            var RecAndUsers = AmatziaDB.Recepies.Join(AmatziaDB.Users,
                r => r.Id,
                u => u.UserId,
                (r, u) => new
                            {
                                RecId = r.Id,
                                UserId = u.UserId,
                                Gender = u.Gender
                            }).ToList();

            var RecRecommandedAndUsers = RecAndUsers;

            foreach (var item in RecAndUsers)
            {
                if (RecRecepies.Where(x => x.Id == item.RecId).Count() == 0)
                {
                    RecRecommandedAndUsers.Remove(item);
                }
            }

            var GenderGroups = RecRecommandedAndUsers.GroupBy(x => x.Gender).Select(group => new
            {
                gender = group.Key,
                count = group.Count()
            }).AsEnumerable();

            return Json(GenderGroups);
        }

    }
}
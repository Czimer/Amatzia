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
            List<Recepie> RecRecepies = new CommentController().GetRecommended();

            var RecAndUsers = RecRecepies.Join(AmatziaDB.Users,
                r => r.UserId,
                u => u.UserId,
                (r, u) => new
                            {
                                RecId = r.Id,
                                UserId = u.UserId,
                                Gender = u.Gender
                            }).ToList();

            var GenderGroups = RecAndUsers.GroupBy(x => x.Gender).Select(group => new
            {
                gender = group.Key,
                count = group.Count()
            }).AsEnumerable();

            return Json(GenderGroups, JsonRequestBehavior.AllowGet);
        }

    }
}
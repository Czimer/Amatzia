using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Amatzia.Models;

namespace Amatzia.Controllers
{
    public class PostController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        public ActionResult Index()
        {
            ViewBag.Selected = "Blog";

            IEnumerable<Recepie> recepiesToShow = (IEnumerable<Recepie>)TempData["Recepie"] ?? AmatziaDB.Recepies.Include("Comments").Include("User").ToList();

            return View(recepiesToShow);
        }

    }
}
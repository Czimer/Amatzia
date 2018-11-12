using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Amatzia.Models;
using Amatzia.Utils;

namespace Amatzia.Controllers
{
    public class PostController : Controller
    {
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        [HttpGet]
        public ActionResult Index(int? Recommended)
        {
            if (Recommended != null)
            {
                return View(this.GetRecommended());

            }
            ViewBag.Selected = "Blog";

            IEnumerable<Recepie> recepiesToShow = (IEnumerable<Recepie>)TempData["Recepie"] ?? AmatziaDB.Recepies.Include("Comments").Include("User").ToList();

            return View(recepiesToShow);
        }

        public List<Models.Recepie> GetRecommended()
        {
            try
            {
                List<Models.Recepie> lstRecepies = new List<Recepie>();
                var recipe = AmatziaDB.Recepies.Include("Comments").ToList();
                foreach (var rcp in recipe)
                {
                    //var comments = System.IO.File.ReadAllLines(@"C:\Users\Bar\Documents\comments.txt");
                    var comments = AmatziaDB.Comments.Where((x) => x.RecepieId == rcp.Id);
                    string comm = string.Empty;
                    foreach (var comment in comments)
                    {
                        //comm = comm + " " + comment;
                        comm = comm + " " + comment.Content;
                    }
                    if (comm != null)
                    {
                        bool bIsRec = MLRecommended.IsRec(comm);
                        if (bIsRec && !lstRecepies.Contains(rcp))
                        {
                            lstRecepies.Add(rcp);
                        }
                    }
                }
                return lstRecepies;
            }
            catch (Exception ex)
            {
                return new List<Recepie>();
            }
        }

        public List<Models.Recepie> GetUnRecommended()
        {
            try
            {
                List<Models.Recepie> lstRecepies = new List<Recepie>();
                var recipe = AmatziaDB.Recepies.ToList();
                foreach (var rcp in recipe)
                {
                    //var comments = System.IO.File.ReadAllLines(@"C:\Users\Bar\Documents\comments.txt");
                    var comments = AmatziaDB.Comments.Where((x) => x.RecepieId == rcp.Id);
                    string comm = string.Empty;
                    foreach (var comment in comments)
                    {
                        //comm = comm + " " + comment;
                        comm = comm + " " + comment.Content;
                    }
                    if (comm != null)
                    {
                        bool bIsRec = MLRecommended.IsRec(comm);
                        if (!bIsRec && !lstRecepies.Contains(rcp))
                        {
                            lstRecepies.Add(rcp);
                        }
                    }
                }
                return lstRecepies;
            }
            catch (Exception ex)
            {
                return new List<Recepie>();
            }
        }


    }
}
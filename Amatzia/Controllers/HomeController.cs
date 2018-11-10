using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amatzia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Hi, welcome to Amatzia! You know that suddenly you feel like cooking, preparing something special, or finding out what to do with the ingredients you have found at home, and most importantly, sharing your thoughts about what you have prepared." +
                              "That's why we're here!, We've created Amatzia so you can share your recipes with us, and also share your opinion about someone else's recipes. Here you can offer improvements to existing recipes, options to alternatives ingredients for vegan or vegetarian recipes." +
                              " Or simply enjoy a variety of recipes published by people." +
                              "Amatzia is a social blog for recipes, because there is nothing more joyful than eating.";

            ViewBag.MessageFoodie = "Foodies are people who really love food. They usually have a special knowledge of food and food preparation techniques. They tend to pride themselves on knowing how to order (and cook) the latest trends. If you are on this site you are in the right path to become a Foodie.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
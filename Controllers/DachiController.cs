using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dojodachi.Controllers
{
    public class DachiController : Controller
    {
        Random rand = new Random();
        public string likedmessage;
        [HttpGet]
        [Route("dojodachi")]
        public IActionResult Index()
        {
            int happy = HttpContext.Session.GetInt32("happyNum")?? 20;
            int full = HttpContext.Session.GetInt32("fullNum")?? 20;
            int energy = HttpContext.Session.GetInt32("energyNum")?? 50;
            int meals = HttpContext.Session.GetInt32("mealsNum")?? 3;
            HttpContext.Session.SetInt32("happyNum", happy);
            ViewBag.Happiness = happy;
            HttpContext.Session.SetInt32("fullNum", full);
            ViewBag.Fullness = full;
            HttpContext.Session.SetInt32("energyNum", energy);
            ViewBag.Energy = energy;
            HttpContext.Session.SetInt32("mealsNum", meals);
            ViewBag.Meals = meals;
            return View("index");
        }

        [HttpPost]
        [Route("sleep")]
        public IActionResult Sleep()
        {
            int? energy = HttpContext.Session.GetInt32("energyNum");
            HttpContext.Session.SetInt32("energyNum", (int)energy + 15);
            int? full = HttpContext.Session.GetInt32("fullNum");
            HttpContext.Session.SetInt32("fullNum", (int)full - 5);
            int? happy = HttpContext.Session.GetInt32("happyNum");
            HttpContext.Session.SetInt32("happyNum", (int)happy - 5);
            TempData["message"] = "Sleeping earns 15 energy and decreases fullness and happiness each by 5";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("play")]
        public IActionResult Play()
        {
            int r = rand.Next(5, 10);
            int? happy = HttpContext.Session.GetInt32("happyNum");
            likedmessage = "Dojodachi doesn`t like to play right now..-5 energy and 0 happiness";
            if(Like())
            {
                HttpContext.Session.SetInt32("happyNum", (int)happy + r);
                TempData["message"] = $"Playing with your Dojodachi costs 5 energy and gains {r} happiness";
            }
            int? energy = HttpContext.Session.GetInt32("energyNum");
            HttpContext.Session.SetInt32("energyNum", (int)energy - 5);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("work")]
        public IActionResult Work()
        {
            int r = rand.Next(1, 4);
            int? energy = HttpContext.Session.GetInt32("energyNum");
            HttpContext.Session.SetInt32("energyNum", (int)energy - 5);
            int? meals = HttpContext.Session.GetInt32("mealsNum");
            HttpContext.Session.SetInt32("mealsNum", (int)meals + r);
            TempData["message"] = $"Working costs 5 energy and earns {r} meals";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("feed")]
        public IActionResult Feed()
        {
            int r = rand.Next(5, 10);
            int? meal = HttpContext.Session.GetInt32("mealsNum");
            int? fullness = HttpContext.Session.GetInt32("fullNum");
            likedmessage = "Dojodachi doesn`t like the food..-1 meal and 0 fullness";
            if(meal <= 0)
            {
                TempData["message"] ="not enough meals, you cannot feed your Dojodachi";
            }
            else
            {
                HttpContext.Session.SetInt32("mealsNum", (int)meal - 1);
                if(Like())
                {
                    HttpContext.Session.SetInt32("fullNum", (int)fullness + r);
                    TempData["message"] = $"Playing with your Dojodachi costs 1 meal and gains {r} fullness";
                }
            }
            System.Console.WriteLine(meal);
            System.Console.WriteLine(fullness);
            return RedirectToAction("Index");
        }

        public bool Like()
        {
            int r = rand.Next(0, 4);
            if(r == 2)
            {
                TempData["message"] = likedmessage;
                return false;
            }
            return true;
        }

        [HttpPost]
        [Route("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
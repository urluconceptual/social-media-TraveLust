﻿using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CitiesController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(City city)
        {

            if (ModelState.IsValid)
            {
                // code generated by GitHub Copilot
                db.Cities.Add(city);
                db.SaveChanges();
                TempData["message"] = "City added!";
                return RedirectToAction("Index", "Posts");
            }
            else
            {
                return View(city);
            }
        }


    }
}

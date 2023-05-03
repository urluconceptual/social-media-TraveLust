﻿using Microsoft.AspNetCore.Authorization;
using System.Data;
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

        // display all cities
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }


            var cities = from city in db.Cities
                         orderby city.CityName
                         select city;
            ViewBag.Cities = cities;
            return View();
        }


        // adding a new city
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public ActionResult New(City city)
        {

            // only admins can add a city directly, otherwise a request is sent to the admin
            if (User.IsInRole("Admin"))
            {
                city.Approved = true;
            }
            else
            {
                city.Approved = false;
            }

            if (ModelState.IsValid)
            {
                // code generated by GitHub Copilot
                db.Cities.Add(city);
                db.SaveChanges();

                // only admins can add a city directly, otherwise a request is sent to the admin
                if (User.IsInRole("Admin"))
                    TempData["message"] = "City added!";
                else
                    TempData["message"] = "A request has been sent to the administrator!";

                return RedirectToAction("Index", "Posts");
            }
            else
            {
                return View(city);
            }
        }

        // editing a city
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Edit(int id)
        {
            City city = db.Cities.Where(c => c.CityId == id).First();
            return View(city);
        }

        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public ActionResult Edit(int id, City requestCity)
        {
            City city = db.Cities.Find(id);

            if (ModelState.IsValid)
            {
                city.CityName = requestCity.CityName;
                city.Country = requestCity.Country;

                db.SaveChanges();
                TempData["message"] = "City edited!";

                return RedirectToAction("Index");
            }
            else
            {
                return View(requestCity);
            }
        }

        // deleting a city
        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            City city = db.Cities.Where(city => city.CityId == id).First();

            // code generated by GitHub Copilot
            db.Cities.Remove(city);
            db.SaveChanges();
            TempData["message"] = "City deleted!";

            return RedirectToAction("Index");
        }

        // approving a city
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Approve(int id)
        {
            City city = db.Cities.Find(id);
            city.Approved = true;
            db.SaveChanges();
            TempData["message"] = "City approved!";
            return RedirectToAction("Index");
        }
    }
}

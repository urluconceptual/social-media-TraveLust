﻿using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class GroupchatsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GroupchatsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // index page displays all groupchats of current user
        //generated by GitHub Copilot
        [Authorize(Roles = "User")]
        [HttpGet("Groupchats/Index/{id1?}/{id2?}")]
        public IActionResult Index(int? id1, int? id2)
        {
            IQueryable<Groupchat> groupchats = db.Groupchats.Include(g => g.UserInGroupchats)
                .Include(g => g.Creator)
                .Where(g => g.UserInGroupchats.Any(u => u.UserId == _userManager.GetUserId(User)));

            ViewBag.Groupchats = groupchats;
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            if (id1 != null)
            {
                Groupchat groupchat = db.Groupchats.Find(id1);
                ViewBag.Groupchat = groupchat;
                IQueryable<Message> messages = db.Messages.Include("User")
                                                          .Where(m => m.GroupchatId == id1)
                                                          .OrderBy(m => m.Date);
                ViewBag.Messages = messages;
            }

            if (id2 != null)
            {
                Itinerary itinerary = db.Itineraries
                    .Include(p => p.PostInItineraries)
                    .ThenInclude(p => p.Votes)
                    .Include(p => p.PostInItineraries)
                    .ThenInclude(p => p.Post)
                    .Where(i => i.ItineraryId == id2).FirstOrDefault();
                ViewBag.Itinerary = itinerary;
            }

            return View();
        }

        // creating a new groupchat
        [Authorize(Roles = "User")]
        public IActionResult New()
        {
            /*generated by GitHub Copilot*/
            Groupchat groupchat = new Groupchat();
            
            return View(groupchat);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> New(Groupchat groupchat)
        {
            groupchat.CreatorId = _userManager.GetUserId(User);


            var sanitizer = new HtmlSanitizer();

            if (ModelState.IsValid)
            {
                groupchat.Description = sanitizer.Sanitize(groupchat.Description);

                db.Groupchats.Add(groupchat);
                db.SaveChanges();

                // generating an itinerary for the groupchat
                // code generated by GitHub Copilot
                Itinerary itinerary = new Itinerary();
                itinerary.GroupchatId = groupchat.GroupchatId;
                itinerary.Budget = 0;
                itinerary.StayingPeriod = 0;
                db.Itineraries.Add(itinerary);
                db.SaveChanges();

                groupchat.ItineraryId = itinerary.ItineraryId;
                db.SaveChanges();


                //groupchat.ItineraryId = db.Itineraries.Where(i => i.GroupchatId == groupchat.GroupchatId).FirstOrDefault().ItineraryID; 

                UserInGroupchat userInGroupchat = new UserInGroupchat();
                userInGroupchat.UserId = _userManager.GetUserId(User);
                userInGroupchat.GroupchatId = groupchat.GroupchatId;
                db.UserInGroupchats.Add(userInGroupchat);

                db.SaveChanges();
                TempData["message"] = "Groupchat created!";

                return RedirectToAction("AddFriends", new {id = groupchat.GroupchatId } );
            }
            else
            {
                return View(groupchat);
            }
        }

        // adding users to a groupchat
        [Authorize(Roles = "User")]
        public IActionResult AddFriends(int id)
        {
            Groupchat groupchat = db.Groupchats.Include("UserInGroupchats").Where(g => g.GroupchatId == id).First();
            groupchat.AllUsers = GetAllUsers();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(groupchat);
        }

        // editing a groupchat 
        [Authorize(Roles = "User")]
        public IActionResult Edit(int id)
        {
            Groupchat groupchat = db.Groupchats.Find(id);
            return View(groupchat);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Edit(int id, Groupchat requestGroupchat)
        {
            Groupchat groupchat = db.Groupchats.Find(id);

            if (ModelState.IsValid)
            {
                groupchat.Name = requestGroupchat.Name;
                groupchat.Description = requestGroupchat.Description;

                db.SaveChanges();
                TempData["message"] = "Groupchat edited!";

                return RedirectToAction("Index");
            }
            else
            {
                return View(requestGroupchat);
            }
        }

        // generated by GitHub Copilot
        // deleting a groupchat
        [Authorize(Roles = "User")]
        public ActionResult Delete(int id)
        {
            Groupchat groupchat = db.Groupchats.Find(id);

            var messages = db.Messages.Where(m => m.GroupchatId == id);

            db.Messages.RemoveRange(messages);
            db.SaveChanges();

            db.Groupchats.Remove(groupchat);
            db.SaveChanges();
            TempData["message"] = "Groupchat deleted!";
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllUsers()
        {
            var selectList = new List<SelectListItem>();

            var users = db.Users.Where(u => u.Id != _userManager.GetUserId(User));

            foreach (var user in users)
            {
                selectList.Add(new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = user.UserName.ToString()
                });
            }
            return selectList;
        }
    }
}

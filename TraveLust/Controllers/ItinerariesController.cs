using Ganss.Xss;
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
    public class ItinerariesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ItinerariesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "User")]
        public IActionResult New(int id)
        {
            Itinerary itinerary = db.Itineraries.Include(d => d.Groupchat).Include(d => d.PostInItineraries).Where(i => i.GroupchatId == id).FirstOrDefault();
            itinerary.TopPosts = GetTopPosts();


            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(itinerary);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> New(Itinerary itinerary)
        {

            if (ModelState.IsValid)
            {
                Itinerary oldItinerary = db.Itineraries.Find(itinerary.ItineraryId);

                oldItinerary.StayingPeriod = itinerary.StayingPeriod;
                oldItinerary.Budget = itinerary.Budget;
                db.SaveChanges();

                return RedirectToAction("Index", "Groupchats", new { id = itinerary.GroupchatId });
            }
            else
            {
                return View(itinerary);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetTopPosts()
        {
            var selectList = new List<SelectListItem>();

            var posts = db.Posts.OrderByDescending(p => p.Rating).Take(5);

            foreach (var post in posts)
            {
                selectList.Add(new SelectListItem
                {
                    Value = post.PostId.ToString(),
                    Text = post.Title.ToString() + " - " + post.Rating.ToString()
                });  
            }
            return selectList;
        }
    }
}

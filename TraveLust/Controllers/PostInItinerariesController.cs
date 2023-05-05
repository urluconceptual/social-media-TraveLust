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
    public class PostInItinerariesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PostInItinerariesController(
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
            PostInItinerary postInItinerary = new PostInItinerary();
            postInItinerary.PostId = id;
            postInItinerary.AllItineraries = GetAllItineraries();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(postInItinerary);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult New(PostInItinerary postInItinerary)
        {
            var check = db.PostInItineraries
                .Where(p => p.PostId == postInItinerary.PostId)
                .Where(p => p.ItineraryId == postInItinerary.ItineraryId)
                .FirstOrDefault();
            if (check != null)
            {
                TempData["message"] = "Post already in this itinerary!";
                return RedirectToAction("New", "PostInItineraries", new { id = postInItinerary.PostId });
            }
            db.PostInItineraries.Add(postInItinerary);
            db.SaveChanges();
            TempData["message"] = "Post succesfully added to itinerary!";
            return RedirectToAction("Show", "Posts", new { id = postInItinerary.PostId });
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllItineraries()
        {
            var selectList = new List<SelectListItem>();

            var gcs = db.Groupchats.Include("UserInGroupchats")
                .Where(g => g.UserInGroupchats.Any(u => u.UserId == _userManager.GetUserId(User)))
                .Select(g => g.GroupchatId);

            var itineraries = db.Itineraries
                .Include("Groupchat")
                .Where(i => gcs.Any(g => g == i.GroupchatId));

            foreach (var i in itineraries)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ItineraryID.ToString(),
                    Text = i.Groupchat.Name.ToString()
                });
            }
            return selectList;
        }
    }
}

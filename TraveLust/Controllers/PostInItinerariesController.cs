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
            if (ModelState.IsValid)
            {
                db.PostInItineraries.Add(postInItinerary);
                db.SaveChanges();

                // update the spending of the itinerary
                // adds the price of the post to the spending of the itinerary
                var p = db.PostInItineraries
                              .Include("Post")
                              .Where(i => i.ItineraryId == postInItinerary.ItineraryId)
                              .Select(i => i.Post.Price)
                              .FirstOrDefault();
                Itinerary itinerary = db.Itineraries.Find(postInItinerary.ItineraryId);

                itinerary.Spending += p;
                db.SaveChanges();

                Vote vote = new Vote();
                vote.PostId = postInItinerary.PostId;
                vote.ItineraryId = postInItinerary.ItineraryId;
                vote.UserId = _userManager.GetUserId(User);
                db.Votes.Add(vote);
                db.SaveChanges();

                TempData["message"] = "Post succesfully added to itinerary!";
                return RedirectToAction("Show", "Posts", new { id = postInItinerary.PostId });
            }
            else
            {
                postInItinerary.AllItineraries = GetAllItineraries();
                return View(postInItinerary);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult RemovePost(PostInItinerary postInItinerary)
        {
            PostInItinerary toDelete = db.PostInItineraries.Find(postInItinerary.PostId, postInItinerary.ItineraryId);

            // update the spending of the itinerary
            // subtracts the price of the post from the spending of the itinerary
            var p = db.PostInItineraries
                          .Include("Post")
                          .Where(i => i.ItineraryId == postInItinerary.ItineraryId)
                          .Select(i => i.Post.Price)
                          .FirstOrDefault();
            Itinerary itinerary = db.Itineraries.Find(postInItinerary.ItineraryId);

            itinerary.Spending -= p;
            db.SaveChanges();

            db.PostInItineraries.Remove(toDelete); 
            db.SaveChanges();

            TempData["message"] = "Post removed from itinerary!";
            return RedirectToAction("Index", "Groupchats", new {id1 = itinerary.GroupchatId, id2 = itinerary.ItineraryId });
        }

        //editing the description of a post in the itinerary
        // id1 = postId, id2 = itineraryId
        [Authorize(Roles = "User")]
		[HttpGet("PostInItineraries/Edit/{id1?}/{id2?}")]
		public IActionResult Edit(int id1, int id2)
        {            
            PostInItinerary post = db.PostInItineraries
                                        .Where(p => p.PostId == id1)
                                        .Where(p => p.ItineraryId == id2)
                                        .FirstOrDefault();
            return View(post);
        }

        [Authorize(Roles = "User")]
        [HttpPost("PostInItineraries/Edit/{id1?}/{id2?}")]
		
		public ActionResult Edit(int id1, int id2, PostInItinerary requestPost)
        {
            PostInItinerary post = db.PostInItineraries
										.Where(p => p.ItineraryId == requestPost.ItineraryId)
										.Where(p => p.PostId == requestPost.PostId)
										.FirstOrDefault();
			Itinerary itinerary = db.Itineraries.Find(post.ItineraryId);

            if (ModelState.IsValid)
            {
               post.Description = requestPost.Description;

                db.SaveChanges();
                TempData["message"] = "Post's descripsion edited!";

                return RedirectToAction("Index", "Groupchats", new {id1 = itinerary.GroupchatId, id2 = itinerary.ItineraryId});
            }
            else
            {
                return View(requestPost);
            }
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
                    Value = i.ItineraryId.ToString(),
                    Text = i.Groupchat.Name.ToString()
                });
            }
            return selectList;
        }

        [NonAction]
        public bool isConfirmed(PostInItinerary postInItinerary)
        {
            Itinerary itinerary = db.Itineraries.Include("Votes").Where(i => i.ItineraryId == postInItinerary.ItineraryId).FirstOrDefault();
            Groupchat groupchat = db.Groupchats.Include("UserInGroupchats").Where(g => g.GroupchatId == itinerary.GroupchatId)
                .FirstOrDefault();
            return postInItinerary.Votes.Count == groupchat.UserInGroupchats.Count;

        }   
    }
}

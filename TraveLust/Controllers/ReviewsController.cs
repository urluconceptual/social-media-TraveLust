using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TraveLust.Data;
using TraveLust.Migrations;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        //adding a review
        public IActionResult New(int id)
        {
            Review review = new Review();
            ViewBag.SightName = db.Posts.Find(id).Title;
            review.PostId = id;
            return View(review);
        }

        [HttpPost]
        public IActionResult New(int id, Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now;
                review.UserId = _userManager.GetUserId(User);
                db.Reviews.Add(review);
                db.SaveChanges();
                ReviewRating(review.PostId);
                TempData["message"] = "Thank you for reviewing this sight!";
                return Redirect("/Posts/Show/" + id);
            }
            else
            {
                return View(review);
            }
        }

        //editing a review
        public IActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);

            if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(review);
            }
            else
            {
                TempData["message"] = "You're not authorized to edit this comment!";
                return Redirect("/Posts/Show/" + review.PostId);
            }

        }

        [HttpPost]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review review = db.Reviews.Find(id);

            if (ModelState.IsValid)
            {
                if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    review.Text = requestReview.Text;
                    review.Rating = requestReview.Rating;
                    review.Date = DateTime.Now;
                    TempData["message"] = "The review was changed!";
                    db.SaveChanges();
                    ReviewRating(review.PostId);

                    return Redirect("/Posts/Show/" + review.PostId);
                }
                else
                {
                    TempData["message"] = "You're not authorized to edit this comment!";
                    return RedirectToAction("/Posts/Show/" + review.PostId);
                }


            }
            else
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                ViewBag.Errors = errors;
                return View(requestReview);
            }
        }

        //deleting a review
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);

            if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
                ReviewRating(review.PostId);
                TempData["message"] = "The review was deleted!";
                return Redirect("/Posts/Show/" + review.PostId);
            }
            else
            {
                TempData["message"] = "You're not authorized to delete this comment!";
                return RedirectToAction("/Posts/Show/" + review.PostId);
            }
        }

        private void ReviewRating(int? id)
        {
            var post = db.Posts.Find(id);
            int nrReviews = db.Reviews.Where(r => r.PostId == id).Count();
            int newRating = 0;
            if (nrReviews == 0)
            {
                post.Rating = 0;
            }
            else
            {
                foreach(var review in db.Reviews.Where(r => r.PostId == id))
                {
                    newRating += review.Rating;
                }
                post.Rating = ((int)((float)newRating / nrReviews*10))/(float)10;
            }
            db.SaveChanges();
        }

    }
}

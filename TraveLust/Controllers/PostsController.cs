using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class PostsController : Controller
    {

        private readonly ApplicationDbContext db;

        public PostsController(
        ApplicationDbContext context
        )
        {
            db = context;
        }

        public IActionResult Index()
        {
            var posts = db.Posts.Include("City");

            ViewBag.Posts = posts;

            return View();
        }

        public IActionResult Show(int id)
        {
            Post post = db.Posts.Include("City")
                                .Where(p => p.PostId == id).First();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(post);
        }
    }
}

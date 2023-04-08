using Microsoft.AspNetCore.Mvc;
using TraveLust.Data;

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
            var posts = db.Posts;

            ViewBag.Posts = posts;

            return View();
        }
    }
}

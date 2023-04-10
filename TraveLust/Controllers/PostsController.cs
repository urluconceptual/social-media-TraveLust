using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var posts = db.Posts.Include("City");

            ViewBag.Posts = posts;

            return View();
        }
    }
}

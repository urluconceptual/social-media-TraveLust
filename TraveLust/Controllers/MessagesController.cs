using Microsoft.AspNetCore.Mvc;

namespace TraveLust.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}

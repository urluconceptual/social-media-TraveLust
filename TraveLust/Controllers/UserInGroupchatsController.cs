using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TraveLust.Data;
using TraveLust.Models;

namespace TraveLust.Controllers
{
    public class UserInGroupchatsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserInGroupchatsController(
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
        [HttpPost]
        public ActionResult New(UserInGroupchat userInGroupchat)
        {
            db.UserInGroupchats.Add(userInGroupchat);
            db.SaveChanges();
            TempData["messageCart"] = "Friend succesfully added to groupchat!";
            return RedirectToAction("AddFriends", "Groupchats", new { id = userInGroupchat.GroupchatId });
        }

        [Authorize(Roles = "User")]
        [HttpPost]
		public ActionResult Delete(UserInGroupchat userInGroupchat)
		{
            UserInGroupchat toDelete = db.UserInGroupchats.Find(userInGroupchat.UserId, userInGroupchat.GroupchatId);
            Groupchat groupchat = db.Groupchats.Find(userInGroupchat.GroupchatId);
            db.UserInGroupchats.Remove(toDelete);
		    db.SaveChanges();
			TempData["message"] = "Groupchat left! You're no longer a member of " + groupchat.Name + ".";

			return RedirectToAction("Index", "Groupchats");
		}

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult RemoveUser(UserInGroupchat userInGroupchat)
        {
            UserInGroupchat toDelete = db.UserInGroupchats.Find(userInGroupchat.UserId, userInGroupchat.GroupchatId);
            Groupchat groupchat = db.Groupchats.Find(userInGroupchat.GroupchatId);
            db.UserInGroupchats.Remove(toDelete);
            db.SaveChanges();
            TempData["message"] = "Member removed from " + groupchat.Name + ".";

            return RedirectToAction("AddFriends", "Groupchats", new {id = groupchat.GroupchatId});
        }
    }
}

﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpPost]
        public ActionResult New(UserInGroupchat userInGroupchat)
        {
            db.UserInGroupchats.Add(userInGroupchat);
            db.SaveChanges();
            TempData["messageCart"] = "Friend succesfully added to groupchat!";
            return RedirectToAction("AddFriends", "Groupchats", new { groupchatId = userInGroupchat.GroupchatId });
        }
    }
}

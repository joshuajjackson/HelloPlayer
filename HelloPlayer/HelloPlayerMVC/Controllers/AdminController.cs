using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelloPlayerMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HelloPlayerMVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;
        // GET: Admin
        public ActionResult Index()
        {
            //return View(db.ApplicationUsers.ToList());
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(userManager.Users.OrderBy(n => n.FamilyName).ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ApplicationUser applicationUser = db.ApplicationUsers.Find(id);
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser applicationUser = userManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var playerMgr = new Logic.PlayerManager();

            ViewBag.Status = userManager.GetRoles(id).First().ToString();
            return View(applicationUser);
        }

        public ActionResult UpgradeToPremium(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string roleFree = "Free Account";
            string rolePremium = "Premium Account";
            ApplicationUser applicationUser = userManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            int playerID = (int)applicationUser.PlayerID;
            var playerMgr = new Logic.PlayerManager();
            var premium = playerMgr.UpgradeToPremium(playerID);
            var addRole = userManager.AddToRole(id, rolePremium);
            var removeRole = userManager.RemoveFromRole(id, roleFree);
            return RedirectToAction("Details", new { id = id });
        }
    }
}

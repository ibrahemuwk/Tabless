using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TablessV1._0.Models;

namespace TablessV1._0.Controllers
{
    public class DbupdateController : Controller
    {
        // GET: Dbupdate
       [HttpPost]
        public ActionResult Index(string Uid)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();


            user.GoogleUserId = Uid;
            user.google_connect = true;

            // save changes to database
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult chk_google()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();


            
            user.google_chk = true;

            // save changes to database
            db.SaveChanges();
            return Json("done google");
        }
        [HttpPost]
        public ActionResult chk_facebook()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.facebook_chk = true;

            // save changes to database
            db.SaveChanges();
            return Json("done facebook");
        }
        [HttpPost]
        public ActionResult chk_twitter()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.twitter_chk = true;

            // save changes to database
            db.SaveChanges();
            return Json("done twitter");
        }
        [HttpPost]
        public ActionResult unchk_google()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.google_chk = false;

            // save changes to database
            db.SaveChanges();
            return Json("done google");
        }
        [HttpPost]
        public ActionResult unchk_facebook()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.facebook_chk = false;

            // save changes to database
            db.SaveChanges();
            return Json("done facebook");
        }
        [HttpPost]
        public ActionResult unchk_twitter()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.twitter_chk = false;

            // save changes to database
            db.SaveChanges();
            return Json("done twitter");
        }
    }
}
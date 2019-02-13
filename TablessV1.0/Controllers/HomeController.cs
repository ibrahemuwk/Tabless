using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TablessV1._0.Models;
using TablessV1._0.Filters;
using TablessV1._0.Extentions;
using TablessV1._0.Controllers;
using Facebook;
using System.Threading.Tasks;

namespace TablessV1._0.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [FacebookAccessToken]
        public  ActionResult Index()
        {


               
            int hour = DateTime.Now.Hour;
            ViewBag.Greating = hour < 12 ? "Good Morning" : "Good Afternoon";
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string firstName=currentUser.Fname;
            string lastName=currentUser.Lname;
            TempData["name"] = firstName+" "+lastName;
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/trains.jpg");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userImage.profile_photo==null)
                {
                    userImage.profile_photo= ImageToByteArray(System.Drawing.Image.FromFile(Server.MapPath("~/Images/5.png")));
                }
                return new FileContentResult(userImage.profile_photo, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/trains.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public FileContentResult Photo(string userId)
        {
            // get EF Database (maybe different way in your applicaiton)
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            return new FileContentResult(user.profile_photo, "image/jpeg");
        }
        public ActionResult twitter_icon()
        {
            // get EF Database (maybe different way in your applicaiton)
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();

           
           
            user.twitter_connect = true;

            // save changes to database
            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }
        public ActionResult google_icon()
        {

            // get EF Database (maybe different way in your applicaiton)
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();


            

            user.google_connect = true;

            // save changes to database
            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }
        public ActionResult facebook_icon()
        {
            // get EF Database (maybe different way in your applicaiton)
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.facebook_connect = true;

            // save changes to database
            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }
    }

}
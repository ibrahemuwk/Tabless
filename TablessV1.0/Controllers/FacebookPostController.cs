using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TablessV1._0.Extentions;
using TablessV1._0.Models;
using TweetSharp;

namespace TablessV1._0.Controllers
{
    [Authorize]
    public class FacebookPostController : BaseController
    {
        private static String _consumerKey = WebConfigurationManager.AppSettings["_consumerKey"];
        private static String _consumerSecret = WebConfigurationManager.AppSettings["_consumerSecret"];
        // GET: FacebookPost
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PostUserFeed(
            string Message,
            string Link,
            string Privacy,
            IList<string> FriendList)
        {
            if (string.IsNullOrEmpty(Message))
            {
                return Json("Can not post a stutas without a message");
            }
            string msg = "";
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            
            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();
            
            if (user.facebook_connect && user.facebook_chk)
            {
                msg=msg+"Post to facebook ";
                
                if (string.IsNullOrEmpty(Privacy))
                {
                    Privacy = "EVERYONE";
                }
                string linkParameter = string.Empty;
                if (!string.IsNullOrEmpty(Link))
                {
                    linkParameter = string.Format("&link={0}", Link);
                }
                string FriendTagList = string.Empty;
                if (FriendList != null)
                {
                    FriendTagList = string.Format("&place=155021662189&tags={0}", string.Join(",", FriendList));
                }
                #region facebook garph api post to user feed

                var access_token = HttpContext.Items["access_token"].ToString();
                if (!string.IsNullOrEmpty(access_token))
                {
                    var appsecret_proof = access_token.generateAppSecretProof();
                    var fb = new FacebookClient(access_token);

                    #region post

                    dynamic myInfo = await fb.PostTaskAsync(
                        (string.Format("me/feed?message={0}{1}{2}",
                        Message,
                        linkParameter,
                        FriendTagList) +
                        "&privacy={{'value':'" + Privacy + "'}}")
                        .GraphAPICall(appsecret_proof), null);
                    #endregion
                }
                else
                    msg= "Missing Access Token";
                #endregion
            }
            if (user.twitter_connect && user.twitter_chk)
            {
                msg = msg + " Post to twitter ";
                #region code for post in twitter

                var service = new TwitterService(_consumerKey, _consumerSecret);
                service.AuthenticateWith(UserToken.Token, UserToken.TokenSecert);
                var result = service.SendTweet(new SendTweetOptions
                {
                    Status = Message
                });
                #endregion


            }
            if (user.google_connect && user.google_chk)
            {
                msg = msg + " Post to google+ ";
            }
            user.twitter_chk = false;
            user.facebook_chk = false;
            user.google_chk = false;
            db.SaveChanges();
            
            return Json(msg);
        }


    }
}
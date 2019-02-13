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
using TablessV1._0.Filters;
using TablessV1._0.Models;
using TweetSharp;

namespace TablessV1._0.Controllers
{
    [Authorize]
    public class FacebookController : BaseController
    {
        public int refresh;
        public int refresh2;
        private static String _consumerKey = WebConfigurationManager.AppSettings["_consumerKey"];
        private static String _consumerSecret = WebConfigurationManager.AppSettings["_consumerSecret"];
        private static String _accessToken = WebConfigurationManager.AppSettings["_accessToken"];
        private static String _accessTokenSecret = WebConfigurationManager.AppSettings["_accessTokenSecret"];
        //public TwitterService service;
        public OAuthAccessToken accessToken;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public async Task<ActionResult> Index(string oauth_token, string oauth_verifier)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();
            if (!user.facebook_connect&&!user.twitter_connect&&!user.google_connect)
            {
                return RedirectToAction("Index", "Manage");
            }
            try
            {

                UserToken.Otoken = oauth_token;
                UserToken.Overifier = oauth_verifier;
                var access_token = Convert.ToString(HttpContext.Items["access_token"]);
                if (access_token != null)
                {
                    try
                    {
                        var appsecret_proof = access_token.generateAppSecretProof();

                        var fb = new FacebookClient(access_token);


                        //Get current user's profile

                        dynamic myInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,link,locale,email,name,birthday,gender,location,age_range".GraphAPICall(appsecret_proof));
                        var facebookProfile = DynamicExtention.ToStatic<FacebookProfileViewModel>(myInfo);
                        dynamic profileImgResult = await fb.GetTaskAsync("{0}/picture?width=200&height=200&redirect=false".GraphAPICall((string)myInfo.id, appsecret_proof));
                        facebookProfile.ImageURL = profileImgResult.data.url;
                        //var facebookProfile = new FacebookProfileViewModel()
                        //{
                        //    FirstName = myInfo.first_name,
                        //    LastName = myInfo.last_name,
                        //    ImageURL = profileImgResult.data.url,
                        //    LinkUrl = myInfo.link,
                        //    Locale = myInfo.locale,
                        //    email = myInfo.email,
                        //    Fullname = myInfo.name,
                        //    gender = myInfo.gender,
                        //    age_range = myInfo.age_range,
                        //    Location = myInfo.location.name,

                        //};
                        return View(facebookProfile);

                    }
                    catch (Exception)
                    {

                        GetFacebookLoginURL();
                    }

                }


            }
            catch (Exception)
            {

                GetFacebookLoginURL();

            }
            return View();
        }

        // action for friends
        public async Task<ActionResult> FB_TaggableFriends()
        {
            var access_token = Convert.ToString(HttpContext.Items["access_token"]);
            if (access_token != null)
            {
                try
                {
                    var appsecret_proof = access_token.generateAppSecretProof();

                    var fb = new FacebookClient(access_token);
                    dynamic myInfo = await fb.GetTaskAsync("me/taggable_friends?fields=id,name,picture.width(500)".GraphAPICall(appsecret_proof));
                    var friendsList = new List<FacebookFriendsViewModel>();
                    foreach (dynamic friend in myInfo.data)
                    {
                        friendsList.Add(DynamicExtention.ToStatic<FacebookFriendsViewModel>(friend));
                    }
                    return PartialView(friendsList);

                }
                catch (Exception) { throw; }
            }
            else
                throw new HttpException(404, "missing access token");
        }
        public async Task<ActionResult> Twitter_GetFeed()
        {
            return PartialView();
        }
        public async Task<ActionResult> Google_GetFeed()
        {
            return PartialView();
        }

        public ActionResult Authorize()
        {
            refresh2 = UserToken.flag2;
            refresh2++;
            UserToken.flag2 = refresh2;
            // Step 1 - Retrieve an OAuth Request Token
           //var temp= FacebookProfileViewModel.iflogedTwitter;
           // temp++;
           // FacebookProfileViewModel.iflogedTwitter = temp;

            TwitterService service = new TwitterService(_consumerKey, _consumerSecret);
            // This is the registered callback URL
            OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:37276/Facebook/Index");

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthenticationUrl(requestToken);

            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            // find the user. I am skipping validations and other checks.
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(x => x.Id == userid).FirstOrDefault();



            user.twitter_connect = true;

            // save changes to database
            db.SaveChanges();

            return JavaScript("window.location = '"+ uri.ToString() + "'");
           //return Redirect(uri.ToString());
        }

        public ActionResult AuthorizeCallback(string oauth_token, string oauth_verifier)
        {
            oauth_token = UserToken.Otoken;
            oauth_verifier = UserToken.Overifier;
            refresh = UserToken.flag;
            refresh += 1;
            UserToken.flag = refresh;
            if (refresh == 1)
            {
                var requestToken = new OAuthRequestToken { Token = oauth_token };

                // Step 3 - Exchange the Request Token for an Access Token
                TwitterService service = new TwitterService(_consumerKey, _consumerSecret);
                accessToken = service.GetAccessToken(requestToken, oauth_verifier);

                // Step 4 - User authenticates using the Access Token
                service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);

                UserToken.Token = accessToken.Token;
                UserToken.TokenSecert = accessToken.TokenSecret;

                TwitterUser user = service.VerifyCredentials(new VerifyCredentialsOptions());
                // IEnumerable<TwitterStatus> tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());

                var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
                var user_tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions());


                //ViewBag.User = user;
                ViewBag.Tweets = tweets;
                ViewBag.UserTweets = user_tweets;
                return View();
            }
            else
            {
                UserToken.flag = 0;
                return RedirectToAction("Authorize");
            }
            //token = oauth_token;
            //verifier = oauth_verifier; 

        }

        public async Task<ActionResult> FB_GetFeed()
        {
            var access_token = Convert.ToString(HttpContext.Items["access_token"]);
            if (access_token != null)
            {
                try
                {
                    var appsecret_proof = access_token.generateAppSecretProof();
                   
                        FacebookProfileViewModel fbm = new FacebookProfileViewModel();
                        var imgurl = fbm.ImageURL;
                    FacebookPostViewModel fbpm = new FacebookPostViewModel();
                    var fb = new FacebookClient(access_token);
                    dynamic myFeed = await fb.GetTaskAsync(
                        ("me/feed?fields=id,from {{id,name,picture{{url}}}},story,picture,link,name,description,message,type,created_time,likes,comments").GraphAPICall(appsecret_proof));
                    var postList = new List<FacebookPostViewModel>();
                    foreach (dynamic post in myFeed.data)
                    {
                        postList.Add(DynamicExtention.ToStatic<FacebookPostViewModel>(post));
                    }
                    fbpm.From_Picture_Url = imgurl;

                    return PartialView(postList);

                }
                catch (Exception) { throw; }
            }
            else
                throw new HttpException(404, "missing access token");
        }
        public async Task<ActionResult> GP_GetFeed()
        {
            try
            {
                return PartialView();
            }
            catch (Exception)
            {

                throw new HttpException(404,"Missing User Id");
            }
        }

        //public async Task<ActionResult> FB_AdminPages()
        //{
        //    var access_token = Convert.ToString(HttpContext.Items["access_token"]);
        //    if (access_token != null)
        //    {

        //            var appsecret_proof = access_token.generateAppSecretProof();

        //            var fb = new FacebookClient(access_token);
        //            dynamic myPages = await fb.GetTaskAsync("me/accounts?fields=id, name, link, is_published, likes, talking_about_count".GraphAPICall(appsecret_proof));
        //            var pageList = new List<FacebookPageViewModel>();
        //            foreach (dynamic page in myPages.data)
        //            {
        //                pageList.Add(DynamicExtention.ToStatic<FacebookPageViewModel>(page));
        //            }
        //            return PartialView(pageList);

        //    }
        //    else
        //        throw new HttpException(404, "missing access token");
        //}
        #region test exceptions

        public async Task<ActionResult> FB_RevokeAccessToken()
        {
            var access_token = HttpContext.Items["access_token"].ToString();
            if (!string.IsNullOrEmpty(access_token))
            {
                var appsecret_proof = access_token.generateAppSecretProof();
                var fb = new FacebookClient(access_token);
                dynamic myFeed = await fb.DeleteTaskAsync(
                    "me/permissions".GraphAPICall(appsecret_proof));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new HttpException(404, "Missing Access Token");
            }
        }
        public ActionResult FB_TestApiLimit()
        {



            return RedirectToAction("Index", "Message",
                new MessageViewModel
                {
                    Type = "Warning",
                    Message = "Facebook Graph API limit reached, Please try again later..."
                });

        }
        #endregion

    }
}
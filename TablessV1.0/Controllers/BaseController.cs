using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using Facebook;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using TablessV1._0.Filters;
using TablessV1._0.Models;
using TablessV1._0.Extentions;
using Newtonsoft.Json;
using TablessV1._0;

namespace TablessV1._0.Controllers
{
    [Authorize]
    [FacebookAccessTokenAttribute]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is FacebookApiLimitException)
            {
                //Status message banner notifying user to try again later
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index", "Message",
                    new MessageViewModel
                    {
                        Type = "Warning",
                        Message = "Facebook Graph API limit reached, Please try again later..."
                    });
            }
            else if (filterContext.Exception is FacebookOAuthException)
                if (HandleAsExpiredToken((FacebookOAuthException)filterContext.Exception))
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = GetFacebookLoginURL();
                }
                else
                {
                    //redirect to Facebook Custom Error Page
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = RedirectToAction("Index", "Message",
                        new MessageViewModel
                        {
                            Type = "Error",
                            Message =
                            string.Format("{0} controller: {1}",
                                    filterContext.Exception.Source,
                                    filterContext.Exception.Message)
                        });
                }
            else if (filterContext.Exception is FacebookApiException)
            {
                //redirect to Facebook Custom Error Page
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index", "Message",
                    new MessageViewModel
                    {
                        Type = "Error",
                        Message =
                            string.Format("{0} controller: {1}",
                                    filterContext.Exception.Source,
                                    filterContext.Exception.Message)
                    });
            }
            else
                base.OnException(filterContext);
        }
        private bool HandleAsExpiredToken(FacebookOAuthException OAuth_ex)
        {
            bool _HandleAsExpiredToken = false;
            if (OAuth_ex.ErrorCode == 190) //OAuthException
            {
                switch (OAuth_ex.ErrorSubcode)
                {
                    case 458: //App Not Installed
                        _HandleAsExpiredToken = true;
                        break;
                    case 459: //User Checkpointed
                        _HandleAsExpiredToken = true;
                        break;
                    case 460: //Password Changed
                        _HandleAsExpiredToken = true;
                        break;
                    case 463: //Expired
                        _HandleAsExpiredToken = true;
                        break;
                    case 464: //Unconfirmed User
                        _HandleAsExpiredToken = true;
                        break;
                    case 467: //Invalid access token
                        _HandleAsExpiredToken = true;
                        break;
                    default:
                        _HandleAsExpiredToken = false;
                        break;
                }
            }
            else if (OAuth_ex.ErrorCode == 102)
            {
                //API Session. Login status or access token has expired, 
                //been revoked, or is otherwise invalid
                _HandleAsExpiredToken = true;
            }
            else if (OAuth_ex.ErrorCode == 10)
            {
                //API Permission Denied. Permission is either not granted or has been removed - 
                //Handle the missing permissions
                _HandleAsExpiredToken = false;
            }
            else if (OAuth_ex.ErrorCode >= 200 && OAuth_ex.ErrorCode <= 299)
            {
                //API Permission (Multiple values depending on permission). 
                //Permission is either not granted or has been removed - Handle the missing permissions
                _HandleAsExpiredToken = false;
            }
            return _HandleAsExpiredToken;
        }
        public RedirectResult GetFacebookLoginURL()
        {
            if (Session["AccessTokenRetryCount"] == null ||
                 (Session["AccessTokenRetryCount"] != null &&
                  Session["AccessTokenRetryCount"].ToString() == "0"))
            {
                Session.Add("AccessTokenRetryCount", "1");
                FacebookClient fb = new FacebookClient();
                fb.AppId = ConfigurationManager.AppSettings["Facebook_AppId"];
                return Redirect(fb.GetLoginUrl(new
                {
                    scope = ConfigurationManager.AppSettings["Facebook_Scope"],
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code"
                }).ToString());
            }
            else
            {
                return Redirect(Url.Action("Index", "Message",
                    new MessageViewModel
                    {
                        Type = "Error",
                        Message = "Unable to obtain a valid Facebook Token after multiple attempts please contact support"
                    }));
            }
        }
        protected Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("ExternalCallBack", "Base");
                return uriBuilder.Uri;
            }
        }
        public async Task<ActionResult> ExternalCallBack(string code)
        {
            //call returned from facbook will include a unique login encrypted code
            //for this user's login with our application id
            //that we can use to optain a new access token
            FacebookClient fb = new FacebookClient();
            //exchange encrypted login code for an access tpken
            dynamic newTokenResult = await fb.GetTaskAsync(
                                     string.Format("oauth/access_token?client_id={0}&client_secret={1}&redirect_uri={2}&code={3}",
                                     ConfigurationManager.AppSettings["Facebook_AppId"],
                                     ConfigurationManager.AppSettings["Facebook_AppSecret"],
                                     Url.Encode(RedirectUri.AbsoluteUri),
                                     code));
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().
                GetUserManager<ApplicationUserManager>();
            if (UserManager != null)
            {
                //Retrive the existing claims for the user and add the facebook access tokenclaim
                var userId = HttpContext.User.Identity.GetUserId();
                IList<Claim> currentClaims = await UserManager.GetClaimsAsync(userId);
                //check to see if a claim already exist for facebookAccessToken
                Claim OlderFacebookAccessTokenClaim = currentClaims.
                    FirstOrDefault(x => x.Type == "FacebookAccessToken");
                //craete new access token claim
                Claim NewFacebookAccessTokenCliam = new Claim("FacebookAccessToken",
                    newTokenResult.access_token);
                if (OlderFacebookAccessTokenClaim == null)
                {
                    //add new facebook Access token Claim
                    await UserManager.AddClaimAsync(userId, NewFacebookAccessTokenCliam);


                }
                else
                {
                    //remove the existing access token claim
                    await UserManager.RemoveClaimAsync(userId, OlderFacebookAccessTokenClaim);
                    //add new facebook Access token claim
                    await UserManager.AddClaimAsync(userId, NewFacebookAccessTokenCliam);
                }
                Session.Add("AccessTokenRetryCount", "0");
            }
            return RedirectToAction("Index", "Facebook");

        }
    }
}
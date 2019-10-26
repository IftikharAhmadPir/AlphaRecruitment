using ORS.API.Models;
using ORS.API.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ORS.API.Entities;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using ORS.API.ViewModels;

namespace ORS.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        private AuthContext db = new AuthContext();
        private UserManager<IdentityUser> _userManager;
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
        public AccountController()
        {
            _repo = new AuthRepository();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(db));
        }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }




        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _repo.RegisterUser(userModel);

            if (result.Succeeded)
            {
                IdentityUser user = await _repo.FindUser(userModel.Email, userModel.Password);

                #region Added Code for Send Email

                string code = await _repo.GenerateEmailConfirmationTokenAsync(user.Id);
                code = HttpUtility.UrlEncode(code);
                try
                {
                    string callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ConfirmEmail", userId = user.Id, code = code });
                    EmailService.SendMail(callbackUrl, user.Email, null);
                    //SendMail(callbackUrl, user.UserName, null);
                }
                catch (Exception ex)
                {
                }
                #endregion
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        #region Confirm Email

        //
        // GET: /Account/ConfirmEmail
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<HttpResponseMessage> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return null;
            }

            code = HttpUtility.UrlDecode(code);

            var result = await _repo.ConfirmEmailAsync(userId, code);

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://alpharecruiters.org/login");
            return response;
        }

        #endregion

        // POST api/Account/changePassword
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                IdentityResult result = await _repo.ChangePassword(vm);

                IHttpActionResult errorResult = GetErrorResult(result);
                if (errorResult != null)
                {
                    return errorResult;
                }
                return Ok();
            }
        }


        // POST api/Account/forgetPassword
        [Route("ForgetPassword")]
        public async Task<IHttpActionResult> ForgetPassword(ResetPasswordViewModel vm)
        {
            if (vm.email == null)
            {
                return BadRequest();
            }
            else
            {
                IdentityUser user = await _userManager.FindByEmailAsync(vm.email);
                string code = await _repo.GeneratePasswordResetTokenAsync(vm.email);

                code = HttpUtility.UrlEncode(code);
                try
                {
                    string callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ResetPassword", userId = user.Id, code = code });
                    EmailService.SendResetPasswordMail(callbackUrl, user.Email, null);
                }
                catch (Exception ex)
                {
                }
                return Ok();
            }
        }

        // GET: /Account/resetPassword
        [Route("ResetPassword")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<HttpResponseMessage> ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return null;
            }
            code = HttpUtility.UrlDecode(code);
            var password = RandomPassword.Generate();
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _repo.ResetPasswordAsync(userId, code, password);
            EmailService.SendTempraryPasswordMail(user.Email, password, null);
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://alpharecruiters.org/login");
            return response;

        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            user = new IdentityUser() { UserName = model.UserName };

            IdentityResult result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            return Ok(accessTokenResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _repo.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "xxxxxx";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            //collect from database if required.
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            //AuthContext db = new AuthContext();
            //var user1 = db.Admin.FirstOrDefault(x => x.UserName == userName);
            //user1.Id;

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion

        #region Sending Mails
        private void SendMail(string code, string username, HttpPostedFileBase fileuploader = null)
        {
            string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "confirmation" + ".cshtml");
            filebody = filebody.Replace("$$name$$", username);
            //filebody = filebody.Replace("$$name$$", "Waseem Ahmad");
            filebody = filebody.Replace("$$code$$", code);
            LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
            logo.ContentId = "companylogo";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
            avHtml.LinkedResources.Add(logo);
            using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], "janiclassical@gmail.com"))
            {
                mail.AlternateViews.Add(avHtml);
                mail.Subject = "Please Confirm Your Account"; 
                mail.Body = "Test attachment";
                mail.Body = filebody;

                if (fileuploader != null)
                {
                    string filename = Path.GetFileName(fileuploader.FileName);
                    mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
                }
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(
                    System.Configuration.ConfigurationManager.AppSettings["UserID"],
                    System.Configuration.ConfigurationManager.AppSettings["Password"]
                    );
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }


        }
        private void SendResetPasswordMail(string code, string username, HttpPostedFileBase fileuploader = null)
        {
            string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "resetpassword" + ".cshtml");
            filebody = filebody.Replace("$$name$$", username);
            //filebody = filebody.Replace("$$name$$", "Waseem Ahmad");
            filebody = filebody.Replace("$$code$$", code);
            LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
            logo.ContentId = "companylogo";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
            avHtml.LinkedResources.Add(logo);
            using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], "janiclassical@gmail.com"))
            {
                mail.AlternateViews.Add(avHtml);
                mail.Subject = "Password Reset";
                mail.Body = "Test attachment";
                mail.Body = filebody;

                if (fileuploader != null)
                {
                    string filename = Path.GetFileName(fileuploader.FileName);
                    mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
                }
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(
                    System.Configuration.ConfigurationManager.AppSettings["UserID"],
                    System.Configuration.ConfigurationManager.AppSettings["Password"]
                    );
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }


        }

        private void SendTempraryPasswordMail(string username, string password, HttpPostedFileBase fileuploader = null)
        {
            string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "temprarypassword" + ".cshtml");
            filebody = filebody.Replace("$$name$$", username);
            //filebody = filebody.Replace("$$name$$", "Waseem Ahmad");
            filebody = filebody.Replace("$$password$$", password);
            LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
            logo.ContentId = "companylogo";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
            avHtml.LinkedResources.Add(logo);
            using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], "janiclassical@gmail.com"))
            {
                mail.AlternateViews.Add(avHtml);
                mail.Subject = "Temprary Password Generated";
                mail.Body = "Test attachment";
                mail.Body = filebody;

                if (fileuploader != null)
                {
                    string filename = Path.GetFileName(fileuploader.FileName);
                    mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
                }
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(
                    System.Configuration.ConfigurationManager.AppSettings["UserID"],
                    System.Configuration.ConfigurationManager.AppSettings["Password"]
                    );
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }


        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PPIJ.Filters;
using PPIJ.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace PPIJ.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            /*if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }*/
            //Needs changing so that it returns AJAX
            System.Text.StringBuilder returnValue = new System.Text.StringBuilder();
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            using (ppijEntities db = new ppijEntities())
            {
                string username = model.Username;
                string password = "";
                var data = System.Text.Encoding.UTF8.GetBytes(model.Password);
                using (SHA256 shaM = new SHA256Managed())
                {
                    byte[] hashBytes = shaM.ComputeHash(data);
                    password = Convert.ToBase64String(hashBytes);

                }

                bool userValid = db.korisnik.Any(user => user.korisnicko_ime == username && user.lozinka == password);

                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    var FormsAuthCookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    var ExistingTicket = FormsAuthentication.Decrypt(FormsAuthCookie.Value).Name;
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/"))
                        return RedirectToLocal(returnUrl);
                    else return JavaScript("location.reload(true)");
                }
                else
                {
                    model.Password = string.Empty;
                    if (returnValue.Length != 0) returnValue.Append("</br/>");
                    returnValue.Append("Korisničko ime ili lozinka su neispravni!");
                }
                return Content(returnValue.ToString());
            }
        }

        public ActionResult Edit()
        {
            UserEditModel model = new UserEditModel();
            using (ppijEntities db = new ppijEntities())
            {
                string username = User.Identity.GetUserName();

                var user = db.korisnik.FirstOrDefault(u => u.korisnicko_ime.Equals(username));
                model.Email = user.email;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditAccount(UserEditModel model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                if (ModelState.IsValid)
                {
                    string username = User.Identity.GetUserName();

                    korisnik trenutniKorisnik = db.korisnik.FirstOrDefault(u => u.korisnicko_ime.Equals(username));

                    //Update fields
                    trenutniKorisnik.email = model.Email;

                    string password = "";

                    if (model.OldPassword != null && model.NewPassword != null && model.NewPasswordConfirm != null)
                    {
                        var data = System.Text.Encoding.UTF8.GetBytes(model.OldPassword);
                        using (SHA256 shaM = new SHA256Managed())
                        {
                            byte[] hashBytes = shaM.ComputeHash(data);
                            password = Convert.ToBase64String(hashBytes);
                        }

                        var data2 = System.Text.Encoding.UTF8.GetBytes(model.NewPassword);
                        var data3 = System.Text.Encoding.UTF8.GetBytes(model.NewPasswordConfirm);
                        using (SHA256 shaM = new SHA256Managed())
                        {
                            byte[] hashBytes2 = shaM.ComputeHash(data2);
                            var test2 = Convert.ToBase64String(hashBytes2);
                            byte[] hashBytes3 = shaM.ComputeHash(data3);
                            var test3 = Convert.ToBase64String(hashBytes3);

                            if (trenutniKorisnik.lozinka == password && test2 == test3)
                            {
                                trenutniKorisnik.lozinka = test2;
                            }
                        }

                    }
                    db.Entry(trenutniKorisnik).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Home"); // or whatever
                }
            }
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return PartialView();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            System.Text.StringBuilder returnValue = new System.Text.StringBuilder();
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(model.TestEmail))
                {
                    returnValue.Append("Ti si računalo!");
                    return Content(returnValue.ToString());
                }
                using (ppijEntities db = new ppijEntities())
                {
                    var newUser = db.korisnik.Create();
                    bool userExists = db.korisnik.Any(user => user.korisnicko_ime == model.Username);
                    bool emailExists = db.korisnik.Any(user=> user.email == model.Email);
                    newUser.korisnicko_ime = model.Username;
                    var data = System.Text.Encoding.UTF8.GetBytes(model.Password);
                    using (SHA256 shaM = new SHA256Managed())
                    {
                        byte[] hashBytes = shaM.ComputeHash(data);
                        newUser.lozinka = Convert.ToBase64String(hashBytes);
                    }
                    newUser.email = model.Email;
                    newUser.ime = model.FirstName;
                    newUser.prezime = model.LastName;
                    if (!userExists && !emailExists)
                    {
                        db.korisnik.Add(newUser);
                        db.SaveChanges();
                        return JavaScript("location.reload(true)");
                    }
                    else { if(userExists)
                    {
                            if (returnValue.Length != 0) returnValue.Append("</br/>");
                            model.Username = string.Empty;
                            returnValue.Append("Korisničko ime " + model.Username + " već postoji!");
                        }
                    if(emailExists)
                    {
                            if (returnValue.Length != 0) returnValue.Append("</br/>");
                            model.Email = string.Empty;

                            returnValue.Append("Već postoji korisnik koji koristi tu email adresu!");
                        }
                    }
                    return Content(returnValue.ToString());
                }
            }
            else
            {
                if (returnValue.Length != 0) returnValue.Append("</br/>");
                returnValue.Append("Podatci nisu ispravni!");
            }

            // If we got this far, something failed, redisplay form
            return PartialView(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Vaša lozinka je imjenjena."
                : message == ManageMessageId.SetPasswordSuccess ? "Vaša lozinka je postavljena."
                : message == ManageMessageId.RemoveLoginSuccess ? "Vanjska prijava je uklonjena."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Stara lozinka je netočna ili nova nije ispravna.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Lokalni račun nije pronađen. Račun sa imenom \"{0}\" možda već postoji.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}

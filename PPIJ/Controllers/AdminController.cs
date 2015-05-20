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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AdminModel model = new AdminModel();
                using (ppijEntities db = new ppijEntities())
                {
                    string username = User.Identity.GetUserName();

                    var user = db.korisnik.FirstOrDefault(u => u.korisnicko_ime.Equals(username));
                    model.Admin = user.administrator;
                    if (!model.Admin)
                    {
                        return RedirectToAction("Login", "Admin");
                    }
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
            
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                AdminModel model = new AdminModel();
                using (ppijEntities db = new ppijEntities())
                {
                    string username = User.Identity.GetUserName();

                    var user = db.korisnik.FirstOrDefault(u => u.korisnicko_ime.Equals(username));
                    model.Admin = user.administrator;
                    if (!model.Admin)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            else
            {
                return View();
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
        // POST: /Admin/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            System.Text.StringBuilder returnValue = new System.Text.StringBuilder();
            if (!ModelState.IsValid)
            {
                return View(model);
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

                bool adminValid = db.korisnik.Any(user => user.korisnicko_ime == username && user.lozinka == password && user.administrator == true);

                if (adminValid)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    var FormsAuthCookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    var ExistingTicket = FormsAuthentication.Decrypt(FormsAuthCookie.Value).Name;
                    return Json(new { RedirectUrl = Url.Action("Index","Admin") });
                }
                else
                {
                    bool userValid = db.korisnik.Any(user => user.korisnicko_ime == username && user.lozinka == password);
                    if (userValid)
                    {
                        if (returnValue.Length != 0) returnValue.Append("</br/>");
                        returnValue.Append("Nemate administratorska prava!");
                    }
                    else
                    {
                        if (returnValue.Length != 0) returnValue.Append("</br/>");
                        returnValue.Append("Korisničko ime ili lozinka su neispravni!");
                    }
                    
                }
                return Content(returnValue.ToString());
            }
        }

        // POST: /Admin/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
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
                    if(trenutniKorisnik.administrator== true)
                    {
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
                        return RedirectToAction("Edit", "Admin"); // or whatever
                    }
                }
            }
            return View(model);
        }
    }
}
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
            return View();
        }

        public ActionResult Login()
        {
            return View();
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
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    if (returnValue.Length != 0) returnValue.Append("</br/>");
                    returnValue.Append("Korisničko ime ili lozinka su neispravni!");
                }
                return Content(returnValue.ToString());
            }
        }
    }
}
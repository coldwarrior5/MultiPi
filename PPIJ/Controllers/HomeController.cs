using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PPIJ.Filters;
using PPIJ.Models;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Data.Entity;

namespace PPIJ.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Exam()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Message(MessageModel model)
        {
            string returnValue = "Poruka je poslana";
            if (!ModelState.IsValid)
            {
                return JavaScript("location.reload(true)");
            }
            string name = model.Name;
            string mailFrom = model.Email;
            string title = model.Title;
            string message = model.Message;
            string mailTo = "codebistro15@gmail.com";
            using (MailMessage mm = new MailMessage(mailFrom, mailTo))
            {
                mm.Subject = title;
                mm.Body = "Message from: " + name + " with email address: " + mailFrom + ".\nMessage: "+ message;
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(mailTo, "ppij2015");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
            
                try { smtp.Send(mm); }
                catch (System.Threading.ThreadAbortException ex) { }
                catch (Exception ex)
                {
                    returnValue = "Greška prilikom slanja";
                }
            }
            ModelState.Clear();
            return Content(returnValue);
        }

    }
}

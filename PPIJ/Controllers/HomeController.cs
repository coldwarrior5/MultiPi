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
using System.Runtime.Serialization.Json;
using System.Text;

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


        [AllowAnonymous]
        public ActionResult ExamPartial()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.podrucje
                             select k).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Areas = query
                };
                return PartialView(model);
            }
        }

        [AllowAnonymous]
        public ActionResult SubjectPartial(int area)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.tema
                             select k).Where(k => k.id_podrucje == area).OrderBy(k => k.id_tema).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Topics = query
                };
                return PartialView(model);
            }
        }

        [AllowAnonymous]
        public ActionResult QuestionPartial(int subject, int min, int max)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.pitanje
                         select k).Where(k => k.id_tema == subject).OrderBy(k => k.id_tema).ToList();
                if(query.Count<min)
                {
                    min =max= query.Count;
                }
                else if (query.Count<max)
                {
                    max = query.Count;
                }
                TablesContentModel model = new TablesContentModel();
                model.minNumQuestions = min;
                model.maxNumQuestions = max;
                return PartialView(model);
            }
        }

        [AllowAnonymous]
        public ActionResult QuestionPartialC(int chosenClass, int min, int max)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.pitanje
                             join u in db.tema on k.id_tema equals u.id_tema
                             select new{k,u.razred}).Where(u => u.razred  == chosenClass).ToList();
                if (query.Count < min)
                {
                    min = max = query.Count;
                }
                else if (query.Count < max)
                {
                    max = query.Count;
                }
                TablesContentModel model = new TablesContentModel();
                model.minNumQuestions = min;
                model.maxNumQuestions = max;
                return PartialView(model);
            }
        }

        [AllowAnonymous]
        public string LoadQuestionsSubject(int idSubject)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query1 = (from c in db.pitanje select c).Where(c => c.id_tema == idSubject).ToList();
                var query2 = (from c in db.odgovor select c).ToList();
                var query3 = (from c in db.slika select c).ToList();
                var query4 = (from c in db.uputa select c).ToList();
                StringBuilder jsonString = new StringBuilder("{ \"quizlist\" : [");
                
                foreach (pitanje element in query1)
                {
                    jsonString.Append("{ \"idQuestion\" : \"");
                    jsonString.Append(element.id_pitanje);
                    jsonString.Append("\",\"question\" : \"");
                    jsonString.Append(element.pitanje1);
                    jsonString.Append("\",\"picture\" : \"");
                    if(element.id_slika!=null)
                    {
                        slika item = query3.Find(x => x.id_slika == element.id_slika);
                        jsonString.Append(item.slika1);
                    }
                    else
                    {
                        jsonString.Append("null");
                    }
                    uputa item2 = query4.Find(x => x.id_uputa == element.id_uputa);
                    jsonString.Append("\",\"idInstruction\" : \"");
                    jsonString.Append(item2.uputa1);
                    jsonString.Append("\",\"singleChoice\" : \"");
                    jsonString.Append(item2.jedan_tocan_odgovor);
                    jsonString.Append("\", \"answers\" : [");
                    IList<odgovor> hits = query2.FindAll(x => x.id_pitanja == element.id_pitanje);
                    
                    foreach(odgovor o in hits)
                    {
                        jsonString.Append("{ \"idAnswer\" : \"");
                        jsonString.Append(o.id_odgovor);
                        jsonString.Append("\",\"answer\" : \"");
                        jsonString.Append(o.odgovor1);
                        jsonString.Append("\",\"picture\" : \"");
                        if (o.id_slika != null)
                        {
                            slika item = query3.Find(x => x.id_slika == o.id_slika);
                            jsonString.Append(item.slika1);
                        }
                        else
                        {
                            jsonString.Append("null");
                        }
                        jsonString.Append("\",\"correct\" : \"");
                        jsonString.Append(o.tocan);
                        jsonString.Append("\"},");
                    }
                    jsonString.Length--;
                    jsonString.Append("]},");
                }
                jsonString.Length--;
                jsonString.Append("]}");
                return jsonString.ToString();
            }
        }

        [AllowAnonymous]
        public string LoadQuestionsClass(int idClass)
        {
            using (ppijEntities db = new ppijEntities())
            {

                var query1 = (from c in db.pitanje
                              join k in db.tema on c.id_tema equals k.id_tema select new {output=c,k.razred}).Where(k => k.razred == idClass).ToList();
                var query2 = (from c in db.odgovor select c).ToList();
                
                var query3 = (from c in db.slika select c).ToList();
                var query4 = (from c in db.uputa select c).ToList();
                StringBuilder jsonString = new StringBuilder("{ \"quizlist\" : [");
                 var ex = query1.ToList();
                 var result = new List<pitanje>();
                 foreach(var bn in ex){
                      result.Add(new pitanje{ id_pitanje = bn.output.id_pitanje, id_slika=bn.output.id_slika,id_tema=bn.output.id_tema,id_uputa=bn.output.id_uputa});
                 }

                foreach (pitanje element2 in result)
                {
                    jsonString.Append("{ \"idQuestion\" : \"");
                    jsonString.Append(element2.id_pitanje);
                    jsonString.Append("\",\"question\" : \"");
                    jsonString.Append(element2.pitanje1);
                    jsonString.Append("\",\"picture\" : \"");
                    if (element2.id_slika != null)
                    {
                        slika item = query3.Find(x => x.id_slika == element2.id_slika);
                        jsonString.Append(item.slika1);
                    }
                    else
                    {
                        jsonString.Append("null");
                    }
                    uputa item2 = query4.Find(x => x.id_uputa == element2.id_uputa);
                    jsonString.Append("\",\"idInstruction\" : \"");
                    jsonString.Append(item2.uputa1);
                    jsonString.Append("\",\"singleChoice\" : \"");
                    jsonString.Append(item2.jedan_tocan_odgovor);
                    jsonString.Append("\", \"answers\" : [");
                    List<odgovor> hits = query2.FindAll(x => x.id_pitanja == element2.id_pitanje);

                    foreach (odgovor o in hits)
                    {
                        jsonString.Append("{ \"idAnswer\" : \"");
                        jsonString.Append(o.id_odgovor);
                        jsonString.Append("\",\"answer\" : \"");
                        jsonString.Append(o.odgovor1);
                        jsonString.Append("\",\"picture\" : \"");
                        if (o.id_slika != null)
                        {
                            slika item = query3.Find(x => x.id_slika == o.id_slika);
                            jsonString.Append(item.slika1);
                        }
                        else
                        {
                            jsonString.Append("null");
                        }
                        jsonString.Append("\",\"correct\" : \"");
                        jsonString.Append(o.tocan);
                        jsonString.Append("\"},");
                    }
                    jsonString.Length--;
                    jsonString.Append("]},");
                }
                jsonString.Length--;
                jsonString.Append("]}");
                return jsonString.ToString();
            }
        }

        // POST: /Home/Index
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Message(MessageModel model)
        {
            string returnValue = "Poruka je poslana";
            if (!ModelState.IsValid)
            {
                return JavaScript("location.reload(true)");
            }
            if (!String.IsNullOrEmpty(model.TestEmail))
            {
                returnValue = "Ti si računalo!";
                ModelState.Clear();
                return Content(returnValue);
            }
            string name = model.Name;
            string mailFrom = model.Email;
            string title = model.Title;
            string message = model.Message;
            string mailTo = "codebistro15@gmail.com";
            using (MailMessage mm = new MailMessage(mailFrom, mailTo))
            {
                MailMessage um = new MailMessage(mailTo, mailFrom);
                mm.Subject = title;
                um.Subject = "Potvrda o zaprimljenom komentaru na MultiPi stranici";
                mm.Body = "Message from: " + name + " with email address: " + mailFrom + ".\nMessage: "+ message;
                um.Body = "Ovo je automatski generirana poruka, nemojte odgovarati na nju.\nNa datum: "+DateTime.Now.ToString()+" smo zaprimili vaš komentar.\nHvala vam na vašem mišljenju, javiti ćemo vam se na: " + mailFrom+ " prema potrebi.\n\nVaš MultiPi tim.";
                mm.IsBodyHtml = false;
                um.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(mailTo, "ppij2015");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
            
                try { smtp.Send(mm);
                }
                catch (System.Threading.ThreadAbortException ex) {
                    ex.ToString();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    returnValue = "Greška prilikom slanja";
                }
                smtp.Send(um);
            }
            
            ModelState.Clear();
            return Content(returnValue);
        }

    }
}

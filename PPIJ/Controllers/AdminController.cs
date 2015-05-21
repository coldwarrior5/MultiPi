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

        [HttpPost, ActionName("Edit")]
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

        public ActionResult Korisnik()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.korisnik
                    select k).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Users = query
                };
                return View(model);
            }
        }

        public ActionResult Odgovor()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.odgovor
                                select k).Include(c => c.pitanje).Include(c => c.slika).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Answers = query
                };
                return View(model);
            }
        }

        public ActionResult Pitanje()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.pitanje
                             select k).Include(c => c.uputa).Include(c => c.tema).Include(c => c.slika).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Questions = query
                };
                return View(model);
            }
        }

        public ActionResult Podrucje()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.podrucje
                             select k).Include(c => c.predmet).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Areas = query
                };
                return View(model);
            }
        }

        public ActionResult Predmet()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.predmet
                             select k).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Subjects = query
                };
                return View(model);
            }
        }

        public ActionResult Slika()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.slika
                             select k).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Pictures = query
                };
                return View(model);
            }
        }

        public ActionResult Tema()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.tema
                             select k).Include(c => c.podrucje).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Topics = query
                };
                return View(model);
            }
        }

        public ActionResult Uputa()
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = (from k in db.uputa
                             select k).ToList();
                TablesContentModel model = new TablesContentModel
                {
                    Instructions = query
                };
                return View(model);
            }
        }

        public ActionResult KorisnikEdit(int id)
        {
            User model = new User();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.korisnik.FirstOrDefault(k => k.id_korisnik.Equals(id));

                model.Username = query.korisnicko_ime;
                model.FirstName = query.ime;
                model.LastName = query.prezime;
                model.Email = query.email;
            }
            return View(model);
        }

        [HttpPost, ActionName("KorisnikEdit")]
        public async Task<ActionResult> KorisnikEditing(int id, User model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                korisnik query = db.korisnik.FirstOrDefault(k => k.id_korisnik.Equals(id));

                query.korisnicko_ime = model.Username;
                query.email = model.Email;
                query.ime = model.FirstName;
                query.prezime = model.LastName;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Korisnik", "Admin");
            }
        }

        public ActionResult KorisnikInsert()
        {
            return View();
        }

        [HttpPost, ActionName("KorisnikInsert")]
        public async Task<ActionResult> KorisnikInserting(User model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.korisnik.Create();

                query.korisnicko_ime = model.Username;
                query.email = model.Email;
                query.ime = model.FirstName;
                query.prezime = model.LastName;
                query.administrator = model.IsAdmin;

                db.korisnik.Add(query);
                db.SaveChanges();
                return RedirectToAction("Korisnik", "Admin");
            }
        }

        public ActionResult OdgovorEdit(int id)
        {
            Answer model = new Answer();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.odgovor.FirstOrDefault(k => k.id_odgovor.Equals(id));

                var query2 = (from p in db.pitanje
                              orderby p.pitanje1
                              select p).ToList();
                model.Questions = new SelectList(query2, "id_pitanje", "pitanje1", query.id_pitanja);

                model.ChosenAnswer = query.odgovor1;
                model.IsCorrect = query.tocan;

            }
            return View(model);
        }

        [HttpPost, ActionName("OdgovorEdit")]
        public async Task<ActionResult> OdgovorEditing(int id, Answer model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.odgovor.FirstOrDefault(k => k.id_odgovor.Equals(id));

                query.odgovor1 = model.ChosenAnswer;
                query.tocan = model.IsCorrect;

                if (Request["QuestionsDD"].Any())
                {
                    var pageSel = Request["QuestionsDD"];
                    query.id_pitanja = Convert.ToInt32(pageSel);
                }

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Odgovor", "Admin");
            }
        }

        public ActionResult PitanjeEdit(int id)
        {
            Question model = new Question();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.pitanje.FirstOrDefault(k => k.id_pitanje.Equals(id));

                var query1 = (from p in db.uputa
                              orderby p.uputa1
                              select p).ToList();
                model.Instructions = new SelectList(query1, "id_uputa", "uputa1", query.id_uputa);

                var query2 = (from p in db.tema
                              orderby p.tema1
                              select p).ToList();
                model.Topics = new SelectList(query2, "id_tema", "tema1", query.id_tema);

                var query3 = (from p in db.slika
                              orderby p.slika1
                              select p).ToList();
                model.Pics = new SelectList(query3, "id_slika", "slika1", query.id_slika);

                model.ChosenQuestion = query.pitanje1;

            }
            return View(model);
        }

        [HttpPost, ActionName("PitanjeEdit")]
        public async Task<ActionResult> PitanjeEditing(int id, Question model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.pitanje.FirstOrDefault(k => k.id_pitanje.Equals(id));

                query.pitanje1 = model.ChosenQuestion;

                if (Request["InstructionsDD"].Any())
                {
                    var uputa = Request["InstructionsDD"];
                    query.id_uputa = Convert.ToInt32(uputa);
                }

                if (Request["TopicsDD"].Any())
                {
                    var tema = Request["TopicsDD"];
                    query.id_tema = Convert.ToInt32(tema);
                }

                if (Request["PicturesDD"].Any())
                {
                    var fotka = Request["PicturesDD"];
                    query.id_slika = Convert.ToInt32(fotka);
                }

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pitanje", "Admin");
            }
        }

        public ActionResult PitanjeInsert()
        {
            Question model = new Question();
            using (ppijEntities db = new ppijEntities())
            {
                var query1 = (from p in db.uputa
                              orderby p.uputa1
                              select p).ToList();
                model.Instructions = new SelectList(query1, "id_uputa", "uputa1");

                var query2 = (from p in db.tema
                              orderby p.tema1
                              select p).ToList();
                model.Topics = new SelectList(query2, "id_tema", "tema1");

                var query3 = (from p in db.slika
                              orderby p.slika1
                              select p).ToList();
                model.Pics = new SelectList(query3, "id_slika", "slika1");
            }
            return View(model);
        }

        [HttpPost, ActionName("PitanjeInsert")]
        public async Task<ActionResult> PitanjeInserting(Question model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.pitanje.Create();

                query.pitanje1 = model.ChosenQuestion;
                if (Request["InstructionsDD"].Any())
                {
                    var uputa = Request["InstructionsDD"];
                    query.id_uputa = Convert.ToInt32(uputa);
                }

                if (Request["TopicsDD"].Any())
                {
                    var tema = Request["TopicsDD"];
                    query.id_tema = Convert.ToInt32(tema);
                }

                if (Request["PicturesDD"].Any())
                {
                    var fotka = Request["PicturesDD"];
                    query.id_slika = Convert.ToInt32(fotka);
                }

                db.pitanje.Add(query);
                db.SaveChanges();
                return RedirectToAction("Pitanje", "Admin");
            }
        }

        public ActionResult PodrucjeEdit(int id)
        {
            Area model = new Area();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.podrucje.FirstOrDefault(k => k.id_podrucje.Equals(id));

                var query1 = (from p in db.predmet
                              orderby p.predmet1
                              select p).ToList();
                model.Subjects = new SelectList(query1, "id_predmet", "predmet1", query.id_predmet);

                model.ChosenArea = query.podrucje1;

            }
            return View(model);
        }

        [HttpPost, ActionName("PodrucjeEdit")]
        public async Task<ActionResult> PodrucjeEditing(int id, Area model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.podrucje.FirstOrDefault(k => k.id_podrucje.Equals(id));

                query.podrucje1 = model.ChosenArea;

                if (Request["SubjectsDD"].Any())
                {
                    var pred = Request["SubjectsDD"];
                    query.id_predmet = Convert.ToInt32(pred);
                }

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Podrucje", "Admin");
            }
        }

        public ActionResult PodrucjeInsert()
        {
            Area model = new Area();
            using (ppijEntities db = new ppijEntities())
            {
                var query1 = (from p in db.predmet
                              orderby p.predmet1
                              select p).ToList();
                model.Subjects = new SelectList(query1, "id_predmet", "predmet1");
            }
            return View(model);
        }

        [HttpPost, ActionName("PodrucjeInsert")]
        public async Task<ActionResult> PodrucjeInserting(Area model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.podrucje.Create();

                query.podrucje1 = model.ChosenArea;

                if (Request["SubjectsDD"].Any())
                {
                    var pred = Request["SubjectsDD"];
                    query.id_predmet = Convert.ToInt32(pred);
                }

                db.podrucje.Add(query);
                db.SaveChanges();
                return RedirectToAction("Podrucje", "Admin");
            }
        }

        public ActionResult PodrucjeRemove(int id)
        {
            return View();
        }

        [HttpPost, ActionName("PodrucjeRemove")]
        public async Task<ActionResult> PodrucjeRemoving(int id)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var podrDelete = db.podrucje.Find(id);
                db.podrucje.Remove(podrDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Podrucje", "Admin");
        }

        public ActionResult PredmetEdit(int id)
        {
            Subject model = new Subject();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.predmet.FirstOrDefault(k => k.id_predmet.Equals(id));

                model.ChosenSubject = query.predmet1;
            }
            return View(model);
        }

        [HttpPost, ActionName("PredmetEdit")]
        public async Task<ActionResult> PredmetEditing(int id, Subject model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.predmet.FirstOrDefault(k => k.id_predmet.Equals(id));

                query.predmet1 = model.ChosenSubject;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Predmet", "Admin");
            }
        }

        public ActionResult PredmetInsert()
        {
            return View();
        }

        [HttpPost, ActionName("PredmetInsert")]
        public async Task<ActionResult> PredmetInserting(Subject model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.predmet.Create();

                query.predmet1 = model.ChosenSubject;

                db.predmet.Add(query);
                db.SaveChanges();
                return RedirectToAction("Predmet", "Admin");
            }
        }

        public ActionResult PredmetRemove(int id)
        {
            return View();
        }

        [HttpPost, ActionName("PredmetRemove")]
        public async Task<ActionResult> PredmetRemoving(int id)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var predDelete = db.predmet.Find(id);
                db.predmet.Remove(predDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Predmet", "Admin");
        }

        public ActionResult SlikaEdit(int id)
        {
            Picture model = new Picture();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.slika.FirstOrDefault(k => k.id_slika.Equals(id));

                model.Pic = query.slika1;
            }
            return View(model);
        }

        [HttpPost, ActionName("SlikaEdit")]
        public async Task<ActionResult> SlikaEditing(int id, Picture model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.slika.FirstOrDefault(k => k.id_slika.Equals(id));

                query.slika1 = model.Pic;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Slika", "Admin");
            }
        }

        public ActionResult SlikaInsert()
        {
            return View();
        }

        [HttpPost, ActionName("SlikaInsert")]
        public async Task<ActionResult> SlikaInserting(Picture model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.slika.Create();

                query.slika1 = model.Pic;
                db.slika.Add(query);
                db.SaveChanges();
                return RedirectToAction("Slika", "Admin");
            }
        }

        public ActionResult SlikaRemove(int id)
        {
            return View();
        }

        [HttpPost, ActionName("SlikaRemove")]
        public async Task<ActionResult> SlikaRemoving(int id, Picture model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var slikaDelete = db.slika.Find(id);
                db.slika.Remove(slikaDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Slika", "Admin");
        }

        public ActionResult TemaEdit(int id)
        {
            Topic model = new Topic();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.tema.FirstOrDefault(k => k.id_tema.Equals(id));

                var query1 = (from p in db.podrucje
                              orderby p.podrucje1
                              select p).ToList();
                model.AvailableAreas = new SelectList(query1, "id_podrucje", "podrucje1", query.id_podrucje);

                model.ChosenTopic = query.tema1;
                model.Description = query.opis;
                model.Class = query.razred;
            }
            return View(model);
        }

        [HttpPost, ActionName("TemaEdit")]
        public async Task<ActionResult> TemaEditing(int id, Topic model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.tema.FirstOrDefault(k => k.id_tema.Equals(id));

                query.tema1 = model.ChosenTopic;
                query.opis = model.Description;
                query.razred = model.Class;

                if (Request["AreasDD"].Any())
                {
                    var podr = Request["AreasDD"];
                    query.id_podrucje = Convert.ToInt32(podr);
                }

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tema", "Admin");
            }
        }

        public ActionResult TemaInsert()
        {
            Topic model = new Topic();
            using (ppijEntities db = new ppijEntities())
            {
                var query1 = (from p in db.podrucje
                              orderby p.podrucje1
                              select p).ToList();
                model.AvailableAreas = new SelectList(query1, "id_podrucje", "podrucje1");
            }
            return View(model);
        }

        [HttpPost, ActionName("TemaInsert")]
        public async Task<ActionResult> TemaInserting(Topic model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.tema.Create();

                query.tema1 = model.ChosenTopic;
                query.opis = model.Description;
                query.razred = model.Class;

                if (Request["AreasDD"].Any())
                {
                    var podr = Request["AreasDD"];
                    query.id_podrucje = Convert.ToInt32(podr);
                }

                db.tema.Add(query);
                db.SaveChanges();
                return RedirectToAction("Tema", "Admin");
            }
        }

        public ActionResult TemaRemove(int id)
        {
            return View();
        }

        [HttpPost, ActionName("TemaRemove")]
        public async Task<ActionResult> TemaRemoving(int id, Topic model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var temaDelete = db.tema.Find(id);
                db.tema.Remove(temaDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Tema", "Admin");
        }

        public ActionResult UputaEdit(int id)
        {
            Instruction model = new Instruction();
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.uputa.FirstOrDefault(k => k.id_uputa.Equals(id));

                model.ChosenInstruction = query.uputa1;
                model.OneCorrect = query.jedan_tocan_odgovor;
            }
            return View(model);
        }

        [HttpPost, ActionName("UputaEdit")]
        public async Task<ActionResult> UputaEditing(int id, Instruction model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.uputa.FirstOrDefault(k => k.id_uputa.Equals(id));

                query.uputa1 = model.ChosenInstruction;
                query.jedan_tocan_odgovor = model.OneCorrect;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Uputa", "Admin");
            }
        }

        public ActionResult UputaInsert()
        {
            return View();
        }

        [HttpPost, ActionName("UputaInsert")]
        public async Task<ActionResult> UputaInserting(Instruction model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var query = db.uputa.Create();

                query.uputa1 = model.ChosenInstruction;
                query.jedan_tocan_odgovor = model.OneCorrect;

                db.uputa.Add(query);
                db.SaveChanges();
                return RedirectToAction("Uputa", "Admin");
            }
        }

        public ActionResult UputaRemove(int id)
        {
            return View();
        }

        [HttpPost, ActionName("UputaRemove")]
        public async Task<ActionResult> UputaRemoving(int id, Instruction model)
        {
            using (ppijEntities db = new ppijEntities())
            {
                var uputaDelete = db.uputa.Find(id);
                db.uputa.Remove(uputaDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Uputa", "Admin");
        }
    }
}
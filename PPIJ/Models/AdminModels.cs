using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using System.Web.Security;

namespace PPIJ.Models
{
    public class AdminModel
    {
        public string Username { get; set; }

        public bool Admin { get; set; }

    }

    public class TablesContentModel
    {
        public IList<korisnik> Users { get; set; }
        public IList<odgovor> Answers { get; set; }
        public IList<pitanje> Questions { get; set; }
        public IList<podrucje> Areas { get; set; }
        public IList<predmet> Subjects { get; set; }
        public IList<slika> Pictures { get; set; }
        public IList<tema> Topics{ get; set; }
        public IList<uputa> Instructions { get; set; }
    }
}
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
using System.Web.Mvc;
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
        public int minNumQuestions { get; set; }
        public int maxNumQuestions { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }
        [Display(Name = "Ime")]
        public string FirstName { get; set; }
        [Display(Name = "Prezime")]
        public string LastName { get; set; }
        [Display(Name = "E-mail adresa")]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Answer
    {
        public int ID { get; set; }
        [Display(Name = "Odabrani odgovor")]
        public string ChosenAnswer { get; set; }
        public int IDpic { get; set; }
        public int IDquestion { get; set; }
        [Display(Name = "Točno?")]
        public bool IsCorrect { get; set; }
        [Display(Name = "Odaberi pitanje:")]
        public SelectList Questions { get; set; }
        [Display(Name = "Odaberi sliku:")]
        public SelectList Pics { get; set; }
    }

    public class Question
    {
        public int ID { get; set; }
        [Display(Name = "Izabrano pitanje:")]
        public string ChosenQuestion { get; set; }
        public int IDpic { get; set; }
        public int IDinstruction { get; set; }
        public int IDtopic { get; set; }
        [Display(Name = "Odaberi uputu:")]
        public SelectList Instructions { get; set; }
        [Display(Name = "Odaberi temu:")]
        public SelectList Topics { get; set; }
        [Display(Name = "Odaberi sliku:")]
        public SelectList Pics { get; set; }
    }

    public class cobined
    {
        public int ID { get; set; }
        public string ChosenQuestion { get; set; }
        public int IDpic { get; set; }
        public int IDinstruction { get; set; }
        public int IDtopic { get; set; }
        public SelectList Instructions { get; set; }
        public SelectList Topics { get; set; }
        public SelectList Pics { get; set; }
        public int Class { get; set; }

    }

    public class Area
    {
        public int ID { get; set; }
        [Display(Name = "Područje:")]
        public string ChosenArea { get; set; }
        public int IDsubject { get; set; }
        [Display(Name = "Odaberi predmet:")]
        public SelectList Subjects { get; set; }
    }

    public class Subject
    {
        public int ID { get; set; }
        [Display(Name = "Predmet:")]
        public string ChosenSubject { get; set; }
    }

    public class Picture
    {
        public int ID { get; set; }
        [Display(Name = "Slika:")]
        public string Pic { get; set; }
    }

    public class Topic
    {
        public int ID { get; set; }
        [Display(Name = "Tema:")]
        public string ChosenTopic { get; set; }
        [Display(Name = "Opis teme:")]
        public string Description { get; set; }
        [Display(Name = "Razred:")]
        public int Class { get; set; }
        public int IDarea { get; set; }
        [Display(Name = "Odaberi područje:")]
        public SelectList AvailableAreas { get; set; }
    }

    public class Instruction
    {
        public int ID { get; set; }
        [Display(Name = "Uputa:")]
        public string ChosenInstruction { get; set; }
        [Display(Name = "Više točnih odgovora?")]
        public bool OneCorrect { get; set; }
    }

}
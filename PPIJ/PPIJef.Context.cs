﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPIJ
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ppijEntities : DbContext
    {
        public ppijEntities()
            : base("name=ppijEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<korisnik> korisnik { get; set; }
        public DbSet<odgovor> odgovor { get; set; }
        public DbSet<pitanje> pitanje { get; set; }
        public DbSet<podrucje> podrucje { get; set; }
        public DbSet<povijest_odgovor> povijest_odgovor { get; set; }
        public DbSet<povijest_pitanje> povijest_pitanje { get; set; }
        public DbSet<povijest_test> povijest_test { get; set; }
        public DbSet<predmet> predmet { get; set; }
        public DbSet<tema> tema { get; set; }
        public DbSet<test> test { get; set; }
        public DbSet<test_pitanje> test_pitanje { get; set; }
        public DbSet<uputa> uputa { get; set; }
        public DbSet<slika> slika { get; set; }
    }
}

//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class test_pitanje
    {
        public test_pitanje()
        {
            this.povijest_pitanje = new HashSet<povijest_pitanje>();
        }
    
        public int id_test_pitanje { get; set; }
        public int multiplikator_bodova { get; set; }
        public int id_pitanje { get; set; }
        public int id_test { get; set; }
    
        public virtual pitanje pitanje { get; set; }
        public virtual ICollection<povijest_pitanje> povijest_pitanje { get; set; }
        public virtual test test { get; set; }
    }
}
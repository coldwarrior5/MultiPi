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
    
    public partial class tema
    {
        public tema()
        {
            this.pitanje = new HashSet<pitanje>();
        }
    
        public int id_tema { get; set; }
        public string tema1 { get; set; }
        public string opis { get; set; }
        public int razred { get; set; }
        public int id_podrucje { get; set; }
    
        public virtual ICollection<pitanje> pitanje { get; set; }
        public virtual podrucje podrucje { get; set; }
    }
}

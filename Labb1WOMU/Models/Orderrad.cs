//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Labb1WOMU.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Orderrad
    {
        [Key]
        public int OrderradID { get; set; }
        public int OrderID { get; set; }
        public int ArtikelID { get; set; }
        public int Antal { get; set; }
    
        public virtual Artikel Artikel { get; set; }
        public virtual Order Order { get; set; }
    }
}

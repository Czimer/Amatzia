//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Amatzia.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recepie
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public Nullable<int> Difficulty { get; set; }
        public System.DateTime UploadDate { get; set; }
        public Nullable<int> duration { get; set; }
    }
}

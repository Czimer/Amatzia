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
    
    public partial class Comment
    {
        public int Id { get; set; }
        public int RecepieId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    
        public virtual Recepie Recepie { get; set; }
        public virtual User User { get; set; }
    }
}

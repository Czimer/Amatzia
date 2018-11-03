using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Amatzia.Utils.Enums;

namespace Amatzia.Models
{
    public class Recepie
    {
        public int id {get; set;}

        public string User { get; set; }

        public string Name { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }

        public int Difficulty { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Upload Date")]
        public DateTime uploadDate {get; set;}
    }
}
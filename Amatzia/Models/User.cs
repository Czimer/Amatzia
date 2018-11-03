using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Amatzia.Utils.Enums;

namespace Amatzia.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public string Country { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
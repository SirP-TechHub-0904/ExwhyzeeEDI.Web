using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Display(Name="Full Name")]
        public string  FullName { get; set; }
        public DateTime Date { get; set; }
        public string  Description { get; set; }
        [Display(Name = "Area Of Specialization")]
        public string  AreaOfSpecialization { get; set; }
        [Display(Name = "About The Author")]
        public string  AboutTheAuthor { get; set; }

        [Display(Name = "Image Path")]
        public string  ImagePath { get; set; }

        
        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }

    }
}
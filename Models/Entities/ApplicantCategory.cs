using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class ApplicantCategory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        [Display(Name = "Category")]
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Description { get; set; }
     
    }
}
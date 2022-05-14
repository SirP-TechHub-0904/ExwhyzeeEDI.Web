using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class School
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Date of Registration")]
        public DateTime DateofRegistration { get; set; }

        public string SchoolNameList
        {
            get
            {
                return SchoolName + " (" + City + ")";
            }
        }
           
    }
}
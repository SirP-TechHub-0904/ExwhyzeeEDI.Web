using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Dtos
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Date of birth (month/day/year)")]
        public DateTime Dateofbirth { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Mode Of Identification")]
        public string ModeOfIdentification { get; set; }
        [Display(Name = "Identification Number")]
        public string IdentificationNumber { get; set; }
        [Display(Name = "Contact Address")]
        public string ContactAddress { get; set; }
        [Display(Name = "State of Origin")]
        public string StateofOrigin { get; set; }
        [Display(Name = "Local Government Area")]
        public string LocalGovernmentArea { get; set; }
        [Display(Name = "Current Occupation")]
        public string CurrentOccupation { get; set; }
        [Display(Name = "Area Of Interest")]
        public string AreaOfInterest { get; set; }
        [Display(Name = "Specific area of interest")]
        public string SpecificAreaOfInterest { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

       
    }
}
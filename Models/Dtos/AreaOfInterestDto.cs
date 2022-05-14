using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Dtos
{
    public class AreaOfInterestDto
    {
        [Display(Name = "Area Of Interest")]
        public string AreaOfInterest { get; set; }
        [Display(Name = "Specific area of interest")]
        public string SpecificAreaOfInterest { get; set; }
    }
}
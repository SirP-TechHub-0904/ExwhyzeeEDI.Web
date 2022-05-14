using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class AboutUsImage
    {
        public int Id { get; set; }
        [Display(Name="Image Name")]
        public string ImageName { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public bool Default { get; set; }


    }
}
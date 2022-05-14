using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public  class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        [Display(Name ="Category")]
        public string CategoryName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        ICollection<Post> Post { get; set; }

    }
}
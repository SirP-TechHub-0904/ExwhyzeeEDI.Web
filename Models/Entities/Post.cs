using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using static ExwhyzeeEDI.Web.Models.Entities.Enum;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Post Title is Required")]
        [Display(Name = "Post Title")]
        public string PostTite { get; set; }
        public int SortOrder { get; set; }
        [Required(ErrorMessage = "Post is Required")]
        [Display(Name = "Post")]
        [AllowHtml]
        public string PostContent { get; set; }

        [AllowHtml]
        public string PostContentPreview { get; set; }
        public string ImageUrl { get; set; }

        [Display(Name = "Post Status")]
        public ProgramStatus Status { get; set; }
        
       
        public DateTime DatePosted { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public ICollection<Comment> Comment { get; set; }
    }
}
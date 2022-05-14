using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ExwhyzeeEDI.Web.Models.Entities.Enum;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class ProgramCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Duration (Months)")]
        public string Duration { get; set; }
        [AllowHtml]
        [Display(Name ="What You Will Learn")]
     
        public string WhatYouWillLearn { get; set; }

        [Display(Name = "Author Name")]
        public int AuthorId { get; set; }
        public Author GetAuthor { get; set; }
        public decimal Amount { get; set; }

        public ProgramStatus Status { get; set; }

        public ICollection<Course> GetCourses { get; set; }
        public string Language { get; set; }

        public ICollection<Review> GetReviews { get; set; }


        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }
    }
}
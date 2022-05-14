using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name="File Path")]
        public string FilePath { get; set; }

        public string Duration { get; set; }

        public int ProgramCourseId { get; set; }
        public ProgramCourse GetProgramCourse { get; set; }


    }
}
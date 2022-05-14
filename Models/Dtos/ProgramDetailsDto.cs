using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Dtos
{
    public class ProgramDetailsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string SchoolName { get; set; }
        public string CourseTitle { get; set; }
        public string Duration { get; set; }
        public DateTime DateRegisterd { get; set; }
        public decimal? Amount { get; set; }
        public int? ProgramCourseId { get; set; }
        public int? SchoolId { get; set; }
        
    }
}
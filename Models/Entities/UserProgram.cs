using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ExwhyzeeEDI.Web.Models.Entities.Enum;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class UserProgram
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public ApplicationUser UserInfo { get; set; }
        public int ProgramCourseId { get; set; }
        public ProgramCourse ProgramCourse { get; set; }
        public DateTime DateRegisterd { get; set; }
        public UserProgramPaymentStatus PaymentStatus { get; set; }
        public CourseStatus CourseStatus { get; set; }
        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }
    }
}
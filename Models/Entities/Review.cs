using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime DateReviewed { get; set; }

        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }
    }
}
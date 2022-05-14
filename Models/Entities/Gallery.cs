using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Gallery
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateUpload { get; set; }
        public int SortOrder { get; set; }

        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }

    }
}
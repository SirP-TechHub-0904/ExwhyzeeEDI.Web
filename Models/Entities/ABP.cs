using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class ABP
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FullNameStyle { get; set; }
        public string RegNumber { get; set; }
        public string RegNumberStyle { get; set; }
        public string CodeStyle { get; set; }
        public string FooterStyle { get; set; }

        public string Note { get; set; }
        public DateTime DateTime { get; set; }
    }
}
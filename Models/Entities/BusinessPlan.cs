using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class BusinessPlan
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        [AllowHtml]
        public string BusinessProfile { get; set; }
        public string BusinessPlanUrl { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
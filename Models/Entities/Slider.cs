using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public string SliderPath { get; set; }
        public int SortOrder { get; set; }

    }
}
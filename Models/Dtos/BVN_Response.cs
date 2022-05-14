using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Dtos
{
   
    public class BVN_Response
    {
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string bvn { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string date_of_birth { get; set; }
        public string phone_number { get; set; }
     
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class CertificateInfo
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Signature { get; set; }
        public string SignatureName { get; set; }
    }
}
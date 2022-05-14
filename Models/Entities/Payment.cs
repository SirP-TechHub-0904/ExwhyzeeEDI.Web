using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ExwhyzeeEDI.Web.Models.Entities.Enum;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? SchoolId { get; set; }
        public int? UserProgramId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime PaymentInitalizedDate { get; set; }
        public DateTime PaymentCompleteDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string TransactionReference { get; set; }
        public string ApprovedBy { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Note { get; set; }
    }
}
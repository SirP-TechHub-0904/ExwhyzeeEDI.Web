using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Enum
    {
      

        public enum ProgramStatus
        {
            [Description("None")]
            None = 1,

            [Description("Published")]
            Published = 2,

            [Description("Unpublished")]
            Unpublished = 3
        }

        public enum UserStatus
        {
            [Description("None")]
            None = 0,

            [Description("Started")]
            Started = 2,

            [Description("Completed")]
            Completed = 3,

            [Description("Pending")]
            Pending = 4
        }

        public enum PaymentStatus
        {
           
            [Description("Successful")]
            Successful = 1,
            [Description("Cancel")]
            Cancel = 2,

            [Description("Pending")]
            Pending = 3,

            [Description("Failed")]
            Failed = 3
        }

        public enum FormStatus
        {
            [Description("Seen")]
            Seen = 1,
            [Description("Not Seen")]
            NotSeen = 2
        }
        public enum EdiStatus
        {

            [Description("Yes")]
            Yes = 1,
            [Description("No")]
            No = 2
        }

        public enum PaymentType
        {
            None = 0,

            [Description("Online Payment")]
            OnlinePayment = 1,

            [Description("Bank Deposit")]
            BankDeposit = 2,

            [Description("By Administrator")]
            ByAdmin = 3,

            
        }

        public enum CourseStatus
        {

            [Description("Completed")]
            Completed = 1,
            [Description("Cancel")]
            Cancel = 2,

            [Description("Active")]
            Active = 3
        }

        public enum UserProgramPaymentStatus
        {

            [Description("Successful")]
            Successful = 1,
            [Description("Cancel")]
            Cancel = 2,

            [Description("Pending")]
            Pending = 3
        }

        //public enum EducationalStatus
        //{
        //    [Description("First School Leaving")]
        //    None = 1,

        //    [Description("JSSCE")]
        //    Undergraduate = 2,

        //    [Description("SSSCE")]
        //    Graduate = 3,

        //    [Description("BSC")]
        //    None = 1,

        //    [Description("HND")]
        //    Undergraduate = 2,

        //    [Description("PGD")]
        //    Graduate = 3


        //}
    }
}
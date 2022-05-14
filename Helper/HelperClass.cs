using ExwhyzeeEDI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Helper
{
    public class HelperClass
    {
        public static bool checkPayment(int id, string userid)
        {
            bool subc = false;
            using (var db = new ApplicationDbContext())
            {
                var sds = db.UserPrograms.FirstOrDefault(x => x.Id == id && x.UserId == userid);
                if (sds == null)
                {
                    return subc = false;
                }
                else
                {
                    if (sds.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Pending)
                    {
                        return subc = false;
                    }
                    else if (sds.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Successful)
                    {
                        return subc = true;
                    }
                }
            }
            return subc;

        }

        public static string GetfullName(string userid)
        {
            string subc = "";
            using (var db = new ApplicationDbContext())
            {
                var sds = db.Profiles.FirstOrDefault(x => x.UserId == userid);
                if (sds == null)
                {
                    return subc = "";
                }
                else
                {
                    return subc = sds.Surname + " " + sds.FirstName + " " + sds.OtherName;
                }
            }
          

        }

        public static string GetSchool(int? id)
        {
            string subc = "";
            using (var db = new ApplicationDbContext())
            {
                var sds = db.Schools.FirstOrDefault(x => x.Id == id);
                if (sds == null)
                {
                    return subc = "";
                }
                else
                {
                    return subc = sds.SchoolName;
                }
            }


        }
    }
}
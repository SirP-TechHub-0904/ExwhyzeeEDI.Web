using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class PartialViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/PartialView
        public ActionResult LayoutProfile()
        {
            var userid = User.Identity.GetUserId();
            var item = db.Profiles.FirstOrDefault(x=>x.UserId == userid);

           

            return PartialView(item);
        }

        public ActionResult LayoutDaashboardIcon()
        {
            try
            {
                var userid = User.Identity.GetUserId();
                var item = db.Profiles.FirstOrDefault(x => x.UserId == userid);
                var school = db.Schools.FirstOrDefault(x => x.Id == item.SchoolId);
                return PartialView(school);

            }
            catch (Exception c)
            {
                return PartialView();

            }
        }

        public ActionResult _AdminDashbar()
        {
           
                //user:
                var prof = db.Profiles.ToList();
                ViewBag.users = prof.Count();

                //paid
                var paidu = db.UserPrograms.Where(x => x.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Successful).ToList();
                ViewBag.paidUsers = paidu.Count();

                //business plan
                var bplan = db.BusinessPlans.ToList();
                ViewBag.businessPlan = bplan.Count();

                //schools
                var sch = db.Schools.ToList();
                ViewBag.sch = sch.Count();

                //
                return PartialView();
            

        }



    }
}
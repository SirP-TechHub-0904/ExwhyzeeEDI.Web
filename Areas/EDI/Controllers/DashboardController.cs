using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ExwhyzeeEDI.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using ExwhyzeeEDI.Web.Models.Dtos;

namespace ExwhyzeeEDI.Web.Areas.EDI.Controllers
{
    public class DashboardController : Controller
    {

        private ApplicationUserManager _userManager;

        // GET: EDI/Dashboard
        private ApplicationDbContext db = new ApplicationDbContext();

        public DashboardController()
        { }
        public DashboardController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            if(user == null)
            {
                return Redirect("/");
            }
            //if(user.ComfirmVerifyCode == false)
            //{
            //    TempData["message"] = "Your account has not been activated";
            //    return RedirectToAction("VerifyAccount", "Data", new { userId = userId, area="", accountinfo = "not activated" });
            //}
            var profile = db.Profiles.FirstOrDefault(x => x.UserId == userId);
            var currentProgram = await db.UserPrograms.Where(x => x.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Pending && x.UserId == userId).FirstOrDefaultAsync();
            if(currentProgram != null)
            {
                //var payment = await db.Payments.FirstOrDefaultAsync(x => x.UserProgramId == currentProgram.Id);

                //programId, int paymentId
                //return RedirectToAction("PayNow", "Payments", new { programId = currentProgram.Id, paymentId = payment.Id, area="EncryptedKey" });

                ViewBag.unpaidAvailable = "Yes";
                return RedirectToAction("RegisteredProgram");
            }

            return View(profile);
        }

        public ActionResult RegisteredProgram()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            if (user == null)
            {
                return Redirect("/");
            }

            var programUser = db.UserPrograms.Include(x=>x.ProgramCourse).Where(x => x.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Pending && x.UserId == userId);
            

                //var output = programUser.Select(x => new ProgramDetailsDto
                //{
                //    ClassLevelName = x.ClassName,
                //    Id = x.Id,
                //    userId = x.UserId,
                //    FormTeacher = x.User.Surname + " " + x.User.FirstName + " " + x.User.OtherName
                //});

            return View(programUser);
        }

        public ActionResult _Profile()
        {
            var user = User.Identity.GetUserId();
            var profile = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == user);
            return PartialView(profile);
        }
    }
}
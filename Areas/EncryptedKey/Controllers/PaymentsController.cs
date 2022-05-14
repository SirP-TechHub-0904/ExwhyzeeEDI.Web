using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using ExwhyzeeEDI.Web.DataServices.PaystackService;
using Microsoft.AspNet.Identity.Owin;
using ExwhyzeeEDI.Web.DataServices.PaymentService;
using Microsoft.AspNet.Identity;

namespace ExwhyzeeEDI.Web.Areas.EncryptedKey.Controllers
{
    public class PaymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IPaystackTransactionService _paystackTransactionService = new PaystackTransactionService();
        private IPaymentServices _paymentServices = new PaymentServices();
        private ApplicationUserManager _userManager;

        public PaymentsController()
        {
        }

        public PaymentsController(PaystackTransactionService paystackTransactionService, ApplicationUserManager userManager,
            PaymentServices paymentServices)
        {
            _paystackTransactionService = paystackTransactionService;
            UserManager = userManager;
            _paymentServices = paymentServices;
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

        // GET: EncryptedKey/Payments
        public async Task<ActionResult> Index()
        {
            return View(await db.Payments.ToListAsync());
        }
        public async Task<ActionResult> PayNow(int programId = 0, int paymentId = 0)
        {
            if(programId == 0)
            {

                return HttpNotFound();  
            }
            var program = await db.UserPrograms.FirstOrDefaultAsync(x => x.Id == programId);
            var user = await UserManager.FindByIdAsync(program.UserId);
            var profile = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == user.Id);

            var programCOurse = await db.ProgramCourses.FirstOrDefaultAsync(x => x.Id == program.ProgramCourseId);

            var payment = await db.Payments.FirstOrDefaultAsync(x => x.Id == paymentId);
            
            if (payment == null)
            {
                return HttpNotFound();
            }
            //
            var secretKey = "sk_live_22586c0490e27d73dd51c8c9d99834b5f7eea922";

            int amountInKobo = (int)programCOurse.Amount * 100;

            var response = await _paystackTransactionService.InitializeTransaction(secretKey, user.Email, amountInKobo, payment.Id, profile.FirstName,
                profile.Surname);

            if (response.status == true)
            {
                return Redirect(response.data.authorization_url);
            }

            return RedirectToAction("PaymentDetails", new { id = payment.Id });

        }
        //
        public ActionResult Invoice(int id)
        {
            var payment = db.Payments.FirstOrDefault(x => x.UserProgramId == id);
            var profile = db.Profiles.Include(x => x.User).Include(x=>x.GetSchool).FirstOrDefault(x => x.UserId == payment.UserId);
            ViewBag.profile = profile;

            var programm = db.UserPrograms.Include(x => x.ProgramCourse).FirstOrDefault(x => x.Id == payment.UserProgramId);
            ViewBag.uprogram = programm;

           double percent = Convert.ToDouble(payment.Amount) * 0.05;
          percent = Math.Round(percent , 2);
            decimal? percentVat = Convert.ToDecimal(percent);
            ViewBag.percentVat = percentVat;

            ViewBag.total = percentVat + payment.Amount;
            return View(payment);
        }
        //
        public async Task<ActionResult> PaymentDetails(int Id)
        {
           
            var payment = await _paymentServices.Get(Id);
            var profile = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == payment.UserId);
            ViewBag.fulname = profile.Surname + " " + profile.FirstName + " " + profile.OtherName;

            
            return View(payment);
        }
        //
        public async Task<ActionResult> Complete()
        {
            //
            //var secretKey = _config["SecretKey"];
            var secretKey = "sk_live_22586c0490e27d73dd51c8c9d99834b5f7eea922";
            var tranxRef = HttpContext.Request["reference"].ToString();
            if (tranxRef != null)
            {
                var response = await _paystackTransactionService.VerifyTransaction(tranxRef, secretKey);

                var id = int.Parse(response.data.metadata.CustomFields.FirstOrDefault(x => x.DisplayName == "Transaction Id").Value);
                var payment = await _paymentServices.Get(id);

                var userid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userid);

                if (response.status)
                {


                    //clientedit.Units = transaction.Amount;

                    if (payment == null)
                    {
                        TempData["warning"] = $"Transaction with Reference {tranxRef} was successful. But Status was not updated. Please contact Help Desk.";
                    }
                    else if (!string.IsNullOrEmpty(payment.TransactionReference))
                    {
                        TempData["warning"] = $"Transaction with Reference {tranxRef} was successful.";
                    }
                    else
                    {


                        payment.PaymentStatus = Models.Entities.Enum.PaymentStatus.Successful;
                        payment.PaymentCompleteDate = DateTime.UtcNow.AddHours(1);
                        payment.TransactionReference = tranxRef;
                        payment.AmountPaid = payment.Amount;
                        payment.PaymentType = Models.Entities.Enum.PaymentType.OnlinePayment;
                        payment.ApprovedBy = "Online";
                        
                        await _paymentServices.Edit(payment);

                        
                        TempData["success"] = $"Transaction with Reference {tranxRef} was successful.";

                    }

                    //
                    var usercourseprogram = await db.UserPrograms.FirstOrDefaultAsync(x => x.Id == payment.UserProgramId);
                    usercourseprogram.PaymentStatus = Models.Entities.Enum.UserProgramPaymentStatus.Successful;
                    db.Entry(usercourseprogram).State = EntityState.Modified;
                    await db.SaveChangesAsync();


                    string txt = $"Your Payment with Reference {tranxRef} was Successful. Welcome to ENTREPRENEURSHIP DEVELOPMENT INSTITUTE.";
                    string msgAdmin = user.Email + " Made a successful Payment";

                    await HelperClass.SendSMS(txt, user.PhoneNumber);
                    await HelperClass.SendMail(user.Email, txt);
                    await HelperClass.SendMail("onwukaemeka41@gmail.com", msgAdmin);
                    await HelperClass.SendMail("judengama@gmail.com", msgAdmin);

                    return RedirectToAction("PaymentDetails", new { id = payment.Id });
                }
                else
                {

                    payment.PaymentStatus = Models.Entities.Enum.PaymentStatus.Failed;
                    payment.TransactionReference = tranxRef;
                    await _paymentServices.Edit(payment);
                    TempData["error"] = $"Transaction with Reference {tranxRef} failed.";

                    return RedirectToAction("PaymentDetails", new { id = payment.Id });

                }
               
            }

 TempData["error"] = $"Transaction with Reference {tranxRef} failed.";

                return RedirectToAction("TransactionHistory");
        }

        // GET: EncryptedKey/Payments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: EncryptedKey/Payments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EncryptedKey/Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,SchoolId,UserProgramId,Amount,PaymentDate,PaymentStatus")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: EncryptedKey/Payments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: EncryptedKey/Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,SchoolId,UserProgramId,Amount,PaymentDate,PaymentStatus")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        // GET: EncryptedKey/Payments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: EncryptedKey/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            db.Payments.Remove(payment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

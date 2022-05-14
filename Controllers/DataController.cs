using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using ExwhyzeeEDI.Web.Models.Dtos;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using static ExwhyzeeEDI.Web.ApplicationSignInManager;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net;
using ExwhyzeeEDI.Web.DataServices.PaymentService;

namespace ExwhyzeeEDI.Web.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;
        private IPaymentServices _paymentServices = new PaymentServices();

        public DataController()
        {
        }

        public DataController(PaymentServices paymentServices, ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _paymentServices = paymentServices;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var user = await UserManager.FindByEmailAsync(model.Email);
            switch (result)
            {

                case SignInStatus.Success:
                    if (User.IsInRole("SuperAdmin,Data,MainList,BossAdmin"))
                    {
                        return RedirectToAction("Index", "Main", new { area = "Admin" });
                    }
                    if (user.Email == "luckman@exwhyzee.ng")
                    {
                        return RedirectToAction("Index", "Luckman", new { area = "Admin" });
                    }
                    //else if (user.ComfirmVerifyCode != true)
                    //{
                    //    //string userId, string accountinfo, string check
                    //    return RedirectToAction("VerifyAccount", new { userId = user.Id });log
                    //}
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        // GET: /Account/Register
        [AllowAnonymous]
        public async Task<ActionResult> CreateRolesTech()
        {

            try
            {

                var role2 = new IdentityRole("XYZTECH");
                var roleresult2 = await RoleManager.CreateAsync(role2);
                if (!roleresult2.Succeeded)
                {
                    ModelState.AddModelError("", roleresult2.Errors.First());
                    //return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            catch (Exception s) { }
            return Content("Row created successfully");

        }

        [AllowAnonymous]
        public async Task<ActionResult> CreateRoles()
        {
            try
            {
                var role = new IdentityRole("Participant");
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    //return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            catch (Exception s) { }

            try
            {

                var role2 = new IdentityRole("Admin");
                var roleresult2 = await RoleManager.CreateAsync(role2);
                if (!roleresult2.Succeeded)
                {
                    ModelState.AddModelError("", roleresult2.Errors.First());
                    //return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            catch (Exception s) { }
            try
            {

                var role3 = new IdentityRole("SuperAdmin");
                var roleresult3 = await RoleManager.CreateAsync(role3);
                if (!roleresult3.Succeeded)
                {
                    ModelState.AddModelError("", roleresult3.Errors.First());
                    //return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                var role4 = new IdentityRole("School");
                var roleresult4 = await RoleManager.CreateAsync(role4);
                if (!roleresult4.Succeeded)
                {
                    ModelState.AddModelError("", roleresult3.Errors.First());
                    //return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            catch (Exception s) { }
            try
            {

                var user = new ApplicationUser { UserName = "super@gmail.com", Email = "super@gmail.com", PhoneNumber = "000" };
                var result = await UserManager.CreateAsync(user, "edi@admin123");
                if (result.Succeeded)
                {
                    //create role


                    await UserManager.AddToRoleAsync(user.Id, "SuperAdmin");
                    await UserManager.AddToRoleAsync(user.Id, "Admin");

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    Profile pro = new Profile();
                    pro.DateRegistered = DateTime.UtcNow;
                    pro.Dateofbirth = DateTime.UtcNow;
                    pro.UserId = user.Id;

                    db.Profiles.Add(pro);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception s) { }
            return Content("Row created successfully");

        }
        // Continue from previous session
        [AllowAnonymous]
        public ActionResult Continue()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Continue(string email, string phone)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempData["msg"] = "Invalid Email or phone number, Start a fresh registration";
                return RedirectToAction("Apply");
            }
            var profile = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (profile == null)
            {
                return RedirectToAction("FillForm", new { userId = user.Id, SchoolId = user.SchoolId });
            }
            var userprogram = await db.UserPrograms.FirstOrDefaultAsync(x => x.UserId == profile.UserId);
            if (userprogram == null)
            {
                return RedirectToAction("CourseRegistration", new { userId = profile.UserId });
            }
            if (user.ComfirmVerifyCode == false)
            {
                return RedirectToAction("VerifyAccount", new { userId = userprogram.UserId, accountinfo = "not activated" });
            }
            else if (user.ComfirmVerifyCode == true)
            {
                return RedirectToAction("AccountInitialization", new { userId = user.Id });
            }
            TempData["error"] = "Invalid email or phone number. Kindly Register";
            return View();
        }
        //
        // GET: /Account/Register
        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public ActionResult Apply()
        {
            ViewBag.ApplicantCategoryId = new SelectList(db.ApplicantCategorys, "Id", "Name");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Data,SuperAdmin")]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Apply(RegisterViewModel model, int ApplicantCategoryId, int SchoolId = 0)
        {
            if (ModelState.IsValid)
            {
                Random _random = new Random();

                var Vnumber = _random.Next(0, 9999).ToString("D4");
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone, VerifyCode = Vnumber, SchoolId = 1 };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create role

                    string[] sio = model.Fullname.Split(' ');

                    await UserManager.AddToRoleAsync(user.Id, "Participant");

                    Profile profile = new Profile();
                   
                    string sn1 = "";
                    string fn1 = "";
                    string ln1 = "";
                    string ol11 = "";
                    if (sio.Count() > 0)
                    {
                        sn1 = sio[0];
                    }
                    if (sio.Count() > 1)
                    {
                        fn1 = sio[1];
                    }
                    if (sio.Count() > 2)
                    {
                        ln1 = sio[2];
                    }
                    if (sio.Count() > 3)
                    {
                        ol11 = sio[3];
                    }
                    profile.Surname = sn1;
                    profile.UserId = user.Id;
                    profile.FirstName = fn1;

                    profile.OtherName = ln1 + " " + ol11;
                    profile.CertificateDate = "07 JAN 2021";
                    profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                    profile.Dateofbirth = DateTime.UtcNow;
                    profile.DateEnrolledForTraining = DateTime.UtcNow;
                    profile.BusinessPlanDateUploaded = DateTime.UtcNow.AddYears(-10);
                    profile.ProgramBy = "Exwhyzee";
                    profile.RegisteredBy = User.Identity.Name;
                    db.Profiles.Add(profile);


                    //
                    ExwhyzeeModel xyz = new ExwhyzeeModel();
                    xyz.Surname = sn1;
                    xyz.UserId = user.Id;
                    xyz.Firstname = fn1;
                    xyz.Othername = ln1 + " " + ol11;
                    xyz.PhoneNumber = model.Phone;
                    xyz.LoanAmount = "3,000,000.00";
                    xyz.LoanTenor = "5";
                    xyz.UserId = user.Id;
                    xyz.BVN = model.BVN;
                    xyz.Date = DateTime.UtcNow.AddHours(1);
                    xyz.ApplicantCategoryId = ApplicantCategoryId;
                    db.ExwhyzeeModels.Add(xyz);
                    await db.SaveChangesAsync();

                    ///

                    string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                    Random num = new Random();

                    // Create new string from the reordered char array
                    string rand = new string(date1.ToCharArray().
                                    OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                    string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                    var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                    datamajor.CertificateId = "EDI/" + rand + date2;
                    string lastnumberid = datamajor.Id.ToString("D4");
                    string number = "EDI/XYZ/2019/" + datamajor.Id;
                    datamajor.RegistrationNumber = number;
                    db.Entry(datamajor).State = EntityState.Modified;

                    ///
                    var xyzdata = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.UserId == profile.UserId);
                    xyzdata.CertificateNumber = "EDI/" + rand + date2;
                    string lastnumberidx = datamajor.Id.ToString("D4");
                    string numberx = "EDI/XYZ/2019/" + datamajor.Id;
                    xyzdata.RegistrationNumber = number;
                    db.Entry(xyzdata).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //return RedirectToAction("FillForm", new { userId = user.Id, SchoolId = user.SchoolId });
                    string admsg = "Fullname: " + model.Fullname + " <br>Email: " + model.Email + " <br> Phone: " + model.Phone;
                    string smsadmsg = "EDI New Applicant \r\n Fullname: " + model.Fullname + " \r\nEmail: " + model.Email + " \r\n Phone: " + model.Phone;
                    try
                    {
                        string message = "";

                        MailMessage mail = new MailMessage();
                        MailMessage mail2 = new MailMessage();

                        //set the addresses 
                        mail.From = new MailAddress("noreply@edi.exwhyzee.ng"); //IMPORTANT: This must be same as your smtp authentication address.

                        //
                        //set the content Server.MapPath("~/status.txt")
                        string AppPath = Request.PhysicalApplicationPath;
                        //StreamReader sr = new StreamReader(AppPath + "../Views/Account/HtmlPage1.html");
                        StreamReader sr = new StreamReader(Server.MapPath("~/Views/Data/Success.html"));

                        mail.Body = sr.ReadToEnd();
                        mail2.Body = sr.ReadToEnd();
                        sr.Close();

                        MailDefinition md = new MailDefinition();
                        md.From = "noreply@edi.exwhyzee.ng";
                        md.IsBodyHtml = true;
                        md.Subject = "New Applicant";

                        //
                        ListDictionary replacements = new ListDictionary();
                        ///
                        replacements.Add("{bHead}", "");
                        replacements.Add("{bbody}", "");
                        replacements.Add("{doyou}", "Admin");
                        //replacements.Add("{subjectt}", "Welcome");
                        replacements.Add("{doyoubody}", "");
                        ///

                        replacements.Add("{activate}", "");
                        replacements.Add("{smalltitle}", admsg);

                        mail = md.CreateMailMessage("ifeanyingama@gmail.com", replacements, mail.Body, new System.Web.UI.Control());



                        //send the message 
                        SmtpClient smtp = new SmtpClient("mail.edi.exwhyzee.ng");

                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        NetworkCredential Credentials = new NetworkCredential("noreply@edi.exwhyzee.ng", "ahambuPeter@123");
                        smtp.Credentials = Credentials;
                        
                        //
                        mail2 = md.CreateMailMessage("onwukaemeka41@gmail.com", replacements, mail2.Body, new System.Web.UI.Control());


                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        smtp.Credentials = Credentials;
                        smtp.Send(mail2);
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {

                        //TempData["mssg"] = "Mail not Sent. Try Again.";
                    }
                    var profilee = db.Profiles.FirstOrDefault(x => x.UserId == user.Id);

                    //try
                    //{

                    //    await HelperClass.SendSMS(admsg, "08144442999");

                    //}
                    //catch (Exception e)
                    //{

                    //}

                    return RedirectToAction("Success");
                    //return Redirect("https://agsmeisapp.nmfb.com.ng/onboarding");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            ViewBag.ApplicantCategoryId = new SelectList(db.Schools, "Id", "Name");
            ViewBag.ApplicantCategoryId = new SelectList(db.ApplicantCategorys, "Id", "Name");

            return View(model);
        }



        /// <summary>
        /// 
        /// 

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult JDPC()
        {
            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JDPC(JDPCRegisterViewModel model, HttpPostedFileBase upload, HttpPostedFileBase upload2, int ApplicantCategoryId = 0, int SchoolId = 0)
        {
            if (ModelState.IsValid)
            {
                Random _random = new Random();

                var Vnumber = _random.Next(0, 9999).ToString("D4");
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone, VerifyCode = Vnumber, SchoolId = 1 };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create role

                    string[] sio = model.Fullname.Split(' ');

                    await UserManager.AddToRoleAsync(user.Id, "Participant");

                    Profile profile = new Profile();

                    string sn1 = "";
                    string fn1 = "";
                    string ln1 = "";
                    string ol11 = "";
                    if (sio.Count() > 0)
                    {
                        sn1 = sio[0];
                    }
                    if (sio.Count() > 1)
                    {
                        fn1 = sio[1];
                    }
                    if (sio.Count() > 2)
                    {
                        ln1 = sio[2];
                    }
                    if (sio.Count() > 3)
                    {
                        ol11 = sio[3];
                    }
                    profile.Surname = sn1;
                    profile.UserId = user.Id;
                    profile.FirstName = fn1;

                    profile.OtherName = ln1 + " " + ol11;
                    profile.CertificateDate = "07 JAN 2021";
                    profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                    profile.Dateofbirth = model.Dateofbirth;
                    profile.StateofOrigin = model.StateofOrigin;
                    profile.LocalGovernmentArea = model.LocalGovernmentArea;
                    profile.ContactAddress = model.ContactAddress;
                    profile.DateEnrolledForTraining = DateTime.UtcNow;
                    profile.BusinessPlanDateUploaded = DateTime.UtcNow.AddYears(-10);
                    profile.ProgramBy = "Exwhyzee";
                    profile.RegisteredBy = User.Identity.Name;
                    db.Profiles.Add(profile);
 ExwhyzeeModel xyz = new ExwhyzeeModel();
                    ///
                    try
                    {

                        if (upload != null && upload.ContentLength > 0)
                        {


                            string date1s = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                            string name = date1s + "-" + profile.Surname;
                            string fileName = Path.GetFileName(name);
                            xyz.PassportPhoto = fileName;
                            fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                            upload.SaveAs(fileName);

                        }

                    }
                    catch (Exception c)
                    {

                    }
                    try
                    {

                        if (upload != null && upload.ContentLength > 0)
                        {


                            string datez1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                            string name = datez1 + "-" + profile.Surname;
                            string fileName = Path.GetFileName(name);
                            xyz.IDUpload = fileName;
                            fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                            upload.SaveAs(fileName);

                        }

                    }
                    catch (Exception c)
                    {

                    }
                    //
                   
                    xyz.Surname = sn1;
                    xyz.UserId = user.Id;
                    xyz.Firstname = fn1;
                    xyz.Othername = ln1 + " " + ol11;
                    xyz.PhoneNumber = model.Phone;
                    xyz.LoanAmount = "3,000,000.00";
                    xyz.LoanTenor = "5";
                    xyz.UserId = user.Id;
                    xyz.BVN = model.BVN;
                    xyz.Sector = model.Sector;
                    xyz.SubSector = model.SubSector;
                    xyz.Parish = model.Parish;
                    xyz.ParishState = model.ParishState;
                    xyz.Date = DateTime.UtcNow.AddHours(1);
                    xyz.ApplicantCategoryId = ApplicantCategoryId;
                    db.ExwhyzeeModels.Add(xyz);
                    await db.SaveChangesAsync();

                    ///

                    string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                    Random num = new Random();

                    // Create new string from the reordered char array
                    string rand = new string(date1.ToCharArray().
                                    OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                    string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                    var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                    datamajor.CertificateId = "EDI/" + rand + date2;
                    string lastnumberid = datamajor.Id.ToString("D4");
                    string number = "EDI/XYZ/2019/" + datamajor.Id;
                    datamajor.RegistrationNumber = number;
                    db.Entry(datamajor).State = EntityState.Modified;

                    ///
                    var xyzdata = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.UserId == profile.UserId);
                    xyzdata.CertificateNumber = "EDI/" + rand + date2;
                    string lastnumberidx = datamajor.Id.ToString("D4");
                    string numberx = "EDI/XYZ/2019/" + datamajor.Id;
                    xyzdata.RegistrationNumber = number;
                    db.Entry(xyzdata).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //return RedirectToAction("FillForm", new { userId = user.Id, SchoolId = user.SchoolId });
                    string admsg = "Fullname: " + model.Fullname + " <br>Email: " + model.Email + " <br> Phone: " + model.Phone;
                    string smsadmsg = "EDI New Applicant \r\n Fullname: " + model.Fullname + " \r\nEmail: " + model.Email + " \r\n Phone: " + model.Phone;
                    try
                    {
                        string message = "";

                        MailMessage mail = new MailMessage();
                        MailMessage mail2 = new MailMessage();

                        //set the addresses 
                        mail.From = new MailAddress("noreply@edi.exwhyzee.ng"); //IMPORTANT: This must be same as your smtp authentication address.

                        //
                        //set the content Server.MapPath("~/status.txt")
                        string AppPath = Request.PhysicalApplicationPath;
                        //StreamReader sr = new StreamReader(AppPath + "../Views/Account/HtmlPage1.html");
                        StreamReader sr = new StreamReader(Server.MapPath("~/Views/Data/Success.html"));

                        mail.Body = sr.ReadToEnd();
                        mail2.Body = sr.ReadToEnd();
                        sr.Close();

                        MailDefinition md = new MailDefinition();
                        md.From = "noreply@edi.exwhyzee.ng";
                        md.IsBodyHtml = true;
                        md.Subject = "New Applicant";

                        //
                        ListDictionary replacements = new ListDictionary();
                        ///
                        replacements.Add("{bHead}", "");
                        replacements.Add("{bbody}", "");
                        replacements.Add("{doyou}", "Admin");
                        //replacements.Add("{subjectt}", "Welcome");
                        replacements.Add("{doyoubody}", "");
                        ///

                        replacements.Add("{activate}", "");
                        replacements.Add("{smalltitle}", admsg);

                        mail = md.CreateMailMessage("ifeanyingama@gmail.com", replacements, mail.Body, new System.Web.UI.Control());



                        //send the message 
                        SmtpClient smtp = new SmtpClient("mail.edi.exwhyzee.ng");

                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        NetworkCredential Credentials = new NetworkCredential("noreply@edi.exwhyzee.ng", "ahambuPeter@123");
                        smtp.Credentials = Credentials;

                        //
                       // mail2 = md.CreateMailMessage("onwukaemeka41@gmail.com", replacements, mail2.Body, new System.Web.UI.Control());


                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        smtp.Credentials = Credentials;
                        smtp.Send(mail2);
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {

                        //TempData["mssg"] = "Mail not Sent. Try Again.";
                    }
                    var profilee = db.Profiles.FirstOrDefault(x => x.UserId == user.Id);

                    //try
                    //{

                    //    await HelperClass.SendSMS(admsg, "08144442999");

                    //}
                    //catch (Exception e)
                    //{

                    //}


                    return RedirectToAction("Success");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName");

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Success()
        {
            //  ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }
        /// 
        /// 
        /// </summary>
        /// 
        /// 
        [Authorize(Roles = "Admin,SuperAdmin")]

        public ActionResult Reg()
        {
            //  ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reg(NewRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.Phone);
                if (check != null)
                {
                    TempData["error"] = "Phone already eexsit";
                    return View(model);
                }
                Random _random = new Random();

                var Vnumber = _random.Next(0, 99999999).ToString("D6");
                string emaild = "";
                if (String.IsNullOrEmpty(model.Email))
                {
                    emaild = "asdf" + Vnumber + "o@xyz.com";
                }
                else
                {
                    emaild = model.Email;
                }
                var user = new ApplicationUser { UserName = emaild, Email = emaild, PhoneNumber = model.Phone, VerifyCode = Vnumber, SchoolId = 1 };
                var result = await UserManager.CreateAsync(user, "1234567890");
                if (result.Succeeded)
                {
                    //create role


                    await UserManager.AddToRoleAsync(user.Id, "Participant");

                    Profile profile = new Profile();
                    profile.Surname = model.Surname;
                    profile.UserId = user.Id;
                    profile.FirstName = model.FirstName;
                    profile.OtherName = model.OtherName;
                    profile.CertificateDate = "07 JAN 2021";
                    profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                    profile.Dateofbirth = DateTime.UtcNow;
                    profile.DateEnrolledForTraining = DateTime.UtcNow;
                    profile.BusinessPlanDateUploaded = DateTime.UtcNow.AddYears(-10);
                    profile.ProgramBy = "luckman";
                    db.Profiles.Add(profile);
                    await db.SaveChangesAsync();

                    ///

                    string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                    Random num = new Random();

                    // Create new string from the reordered char array
                    string rand = new string(date1.ToCharArray().
                                    OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                    string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                    var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                    datamajor.CertificateId = "EDI/" + rand + date2;
                    string lastnumberid = datamajor.Id.ToString("D4");
                    string number = "EDI/XYZ/2019/" + datamajor.Id;
                    datamajor.RegistrationNumber = number;
                    db.Entry(datamajor).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Luckman", new { area = "Admin" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            //   ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View(model);
        }

        /// <summary>


        [Authorize(Roles = "Admin,SuperAdmin")]

        public ActionResult BatchB()
        {
            //  ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BatchB(NewRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.Phone);
                if (check != null)
                {
                    TempData["error"] = "Phone already eexsit";
                    return View(model);
                }
                Random _random = new Random();

                var Vnumber = _random.Next(0, 99999999).ToString("D6");
                string emaild = "";
                if (String.IsNullOrEmpty(model.Email))
                {
                    emaild = "asdf" + Vnumber + "o@xyz.com";
                }
                else
                {
                    emaild = model.Email;
                }
                var user = new ApplicationUser { UserName = emaild, Email = emaild, PhoneNumber = model.Phone, VerifyCode = Vnumber, SchoolId = 1 };
                var result = await UserManager.CreateAsync(user, "1234567890");
                if (result.Succeeded)
                {
                    //create role


                    await UserManager.AddToRoleAsync(user.Id, "Participant");

                    Profile profile = new Profile();
                    profile.Surname = model.Surname;
                    profile.UserId = user.Id;
                    profile.FirstName = model.FirstName;
                    profile.OtherName = model.OtherName;
                    profile.CertificateDate = "07 JAN 2021";
                    profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                    profile.Dateofbirth = DateTime.UtcNow;
                    profile.DateEnrolledForTraining = DateTime.UtcNow;
                    profile.BusinessPlanDateUploaded = DateTime.UtcNow.AddYears(-10);
                    profile.ProgramBy = "batch b";
                    profile.PaidForTraining = true;
                    profile.Attendance = Models.Entities.Enum.EdiStatus.Yes;
                    db.Profiles.Add(profile);
                    await db.SaveChangesAsync();

                    ///

                    string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                    Random num = new Random();

                    // Create new string from the reordered char array
                    string rand = new string(date1.ToCharArray().
                                    OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                    string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                    var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                    datamajor.CertificateId = "EDI/" + rand + date2;
                    string lastnumberid = datamajor.Id.ToString("D4");
                    string number = "EDI/XYZ/2019/" + datamajor.Id;
                    datamajor.RegistrationNumber = number;
                    db.Entry(datamajor).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "BatchB", new { area = "Admin" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            //   ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View(model);
        }


        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<ActionResult> AddUserProfile(string uid)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Users.FirstOrDefaultAsync(x => x.Id == uid);
                if (check != null)
                {
                    //create role


                    await UserManager.AddToRoleAsync(check.Id, "Participant");

                    Profile profile = new Profile();
                    profile.Surname = "";
                    profile.UserId = check.Id;
                    profile.FirstName = "";
                    profile.OtherName = "";
                    profile.CertificateDate = "07 JAN 2021";
                    profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                    profile.Dateofbirth = DateTime.UtcNow;
                    profile.DateEnrolledForTraining = DateTime.UtcNow;
                    profile.BusinessPlanDateUploaded = DateTime.UtcNow.AddYears(-10);
                    profile.ProgramBy = "luckman";
                    db.Profiles.Add(profile);
                    await db.SaveChangesAsync();

                    ///

                    string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                    Random num = new Random();

                    // Create new string from the reordered char array
                    string rand = new string(date1.ToCharArray().
                                    OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                    string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                    var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                    datamajor.CertificateId = "EDI/" + rand + date2;
                    string lastnumberid = datamajor.Id.ToString("D4");
                    string number = "EDI/XYZ/2019/" + datamajor.Id;
                    datamajor.RegistrationNumber = number;
                    db.Entry(datamajor).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Luckman", new { area = "Admin" });
                }
            }

            // If we got this far, something failed, redisplay form

            //   ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        [Authorize(Roles = "Admin,SuperAdmin,Data")]
        public ActionResult SearchNumber()
        {
            //  ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin,Data")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchNumber(string number)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == number);
                if (check != null)
                {
                    var re = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == check.Id);
                    ViewBag.em = check.Email;
                    ViewBag.ph = check.PhoneNumber;
                    ViewBag.uid = check.Id;
                    return View(re);
                }


            }

            // If we got this far, something failed, redisplay form

            //   ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult RegUpdate(string uid)
        {
            ViewBag.uid = uid;
            //  ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegUpdate(string sname, string fname, string othname, string uid)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == uid);
                Profile profile = new Profile();
                profile.Surname = sname;
                profile.UserId = user.Id;
                profile.FirstName = fname;
                profile.OtherName = othname;
                profile.CertificateDate = "07 JAN 2021";
                profile.DateRegistered = DateTime.UtcNow.AddHours(1);
                profile.Dateofbirth = DateTime.UtcNow;
                profile.DateEnrolledForTraining = DateTime.UtcNow;
                db.Profiles.Add(profile);
                await db.SaveChangesAsync();

                ///

                string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                Random num = new Random();

                // Create new string from the reordered char array
                string rand = new string(date1.ToCharArray().
                                OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.Id == profile.Id);
                datamajor.CertificateId = "EDI/" + rand + date2;
                string lastnumberid = datamajor.Id.ToString("D4");
                string number = "EDI/XYZ/2019/" + datamajor.Id;
                datamajor.RegistrationNumber = number;
                db.Entry(datamajor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["s"] = "success";
                return View();
            }
            catch (Exception d)
            {
                TempData["s"] = "error";
                return View();
            }

        }


        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<ActionResult> CertList()
        {
            var cer = await db.Profiles.Include(x => x.User).OrderByDescending(x => x.DateRegistered).ToListAsync();
            return View(cer);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<ActionResult> CertListMain()
        {
            var cer = await db.Profiles.Include(x => x.User).OrderByDescending(x => x.DateRegistered).Take(20).ToListAsync();
            return View(cer);
        }


        /// <returns></returns>
        //register school

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult SchoolRegistration()
        {


            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SchoolRegistration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create role


                    await UserManager.AddToRoleAsync(user.Id, "School");
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("CompleteAccount", "Schools", new { userId = user.Id, area = "Admin" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View(model);
        }


        //profile
        [AllowAnonymous]
        public ActionResult FillForm(string userId, int SchoolId)
        {
            var profile = db.Profiles.FirstOrDefault(x => x.UserId == userId);
            if (profile != null)
            {
                return RedirectToAction("CourseRegistration", new { userId = profile.UserId });
            }
            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName");
            ViewBag.userId = userId;
            ViewBag.SchoolId = SchoolId;
            return View();
        }

        [AllowAnonymous]
        public JsonResult LgaList(string Id)
        {
            var stateId = db.States.FirstOrDefault(x => x.StateName == Id).Id;
            var local = from s in db.LocalGovs
                        where s.StatesId == stateId
                        select s;

            return Json(new SelectList(local.ToArray(), "LGAName", "LGAName"), JsonRequestBehavior.AllowGet);
        }

        // POST: EDI/ProfileAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> FillForm(Profile model, int SchoolId, string userId, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
                if (check != null)
                {
                    return RedirectToAction("CourseRegistration", new { userId = userId });
                }
                try
                {

                    if (upload != null && upload.ContentLength > 0)
                    {


                        string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                        string name = date1 + "-" + upload.FileName;
                        string fileName = Path.GetFileName(name);
                        model.PassportUrl = fileName;
                        fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        upload.SaveAs(fileName);

                    }

                }
                catch (Exception c)
                {

                }
                model.BusinessPlanDateUploaded = DateTime.UtcNow;
                model.DateEnrolledForTraining = DateTime.UtcNow;
                model.DateRegistered = DateTime.UtcNow;
                model.UserId = userId;
                model.SchoolId = SchoolId;
                db.Profiles.Add(model);
                await db.SaveChangesAsync();

                var profileinfo = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
                string lastnumberid = profileinfo.Id.ToString("D4");
                string number = "EDI/XYZ/2019/" + profileinfo.Id;
                profileinfo.RegistrationNumber = number;
                db.Entry(profileinfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var user = await UserManager.FindByIdAsync(profileinfo.UserId);

                return RedirectToAction("CourseRegistration", new { userId = user.Id });
            }
            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName", model.StateofOrigin);
            ViewBag.SchoolId = SchoolId;
            ViewBag.userId = userId;
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult CourseRegistration(string userId)
        {
            var program = db.UserPrograms.FirstOrDefault(x => x.UserId == userId);
            if (program != null)
            {
                return RedirectToAction("VerifyAccount", new { userId = program.UserId });
            }
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title");
            ViewBag.userId = userId;
            return View();
        }

        // POST: EDI/ProfileAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CourseRegistration(UserProgram userProgram, string userId)
        {
            if (ModelState.IsValid)
            {
                var check = await db.UserPrograms.FirstOrDefaultAsync(x => x.ProgramCourseId == userProgram.ProgramCourseId && x.UserId == userId);
                if (check != null)
                {
                    return RedirectToAction("VerifyAccount", new { userId = userId });
                }
                var userSchoolId = await UserManager.FindByIdAsync(userId);
                userProgram.DateRegisterd = DateTime.UtcNow;
                userProgram.UserId = userId;
                userProgram.PaymentStatus = Models.Entities.Enum.UserProgramPaymentStatus.Pending;
                userProgram.SchoolId = userSchoolId.SchoolId;
                db.UserPrograms.Add(userProgram);
                await db.SaveChangesAsync();
                var user = await UserManager.FindByIdAsync(userId);
                //  string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Data", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string callbk = user.VerifyCode;
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                try
                {
                    string message = "";

                    MailMessage mail = new MailMessage();

                    //set the addresses 
                    mail.From = new MailAddress("noreply@edi.exwhyzee.ng"); //IMPORTANT: This must be same as your smtp authentication address.

                    //
                    //set the content Server.MapPath("~/status.txt")
                    string AppPath = Request.PhysicalApplicationPath;
                    //StreamReader sr = new StreamReader(AppPath + "../Views/Account/HtmlPage1.html");
                    StreamReader sr = new StreamReader(Server.MapPath("~/Views/Data/Success.html"));

                    mail.Body = sr.ReadToEnd();
                    sr.Close();

                    MailDefinition md = new MailDefinition();
                    md.From = "noreply@edi.exwhyzee.ng";
                    md.IsBodyHtml = true;
                    md.Subject = "Welcome to EXWHYZEE EDI";

                    //
                    ListDictionary replacements = new ListDictionary();
                    ///
                    replacements.Add("{bHead}", "EXWHYZEE ENTREPRENEURSHIP DEVELOPMENT INSTITUTE");
                    replacements.Add("{bbody}", "MISSION: By the year 2025, and beyond. We must have created atleast 1000 entrepreneurs in the South East and Nation atlarge who are self sustained, This we intend to do by providing world class entrepreneurship training, mentorship, providing access to continous funding and building global market linkage.");
                    replacements.Add("{doyou}", "WELCOME, " + user.UserName);
                    //replacements.Add("{subjectt}", "Welcome");
                    replacements.Add("{doyoubody}", "The Easternpreneurs is a forum and platform put together by project consultants, portfolio business managers, commercial law consultants and investment consultants to provide a network for entrepreneurs, commercial and business agencies and companies for interaction, business roundtable, investors exchange platform and development of international trade and business connections.");
                    ///

                    replacements.Add("{activate}", "Activation Number: " + callbk);
                    replacements.Add("{smalltitle}", "Benefits of E.D.I");

                    mail = md.CreateMailMessage(user.Email, replacements, mail.Body, new System.Web.UI.Control());



                    //send the message 
                    SmtpClient smtp = new SmtpClient("mail.edi.exwhyzee.ng");

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    NetworkCredential Credentials = new NetworkCredential("noreply@edi.exwhyzee.ng", "ahambuPeter@123");
                    smtp.Credentials = Credentials;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {

                    //TempData["mssg"] = "Mail not Sent. Try Again.";
                }
                var profilee = db.Profiles.FirstOrDefault(x => x.UserId == user.Id);

                string msg = "Welcome to EXWHYZEE EDI. Activation_Number: " + callbk;
                string msgAdmin = profilee.Surname + " " + profilee.FirstName + " with phone No. " + user.PhoneNumber + " just registered";
                try
                {
                    await HelperClass.SendSMS(msg, user.PhoneNumber);
                    await HelperClass.SendSMS(msgAdmin, "08063812435");
                    await HelperClass.SendMail("onwukaemeka41@gmail.com", msgAdmin);
                    await HelperClass.SendMail("judengama@gmail.com", msgAdmin);
                    await HelperClass.SendMail("obiekeabb@gmail.com", msgAdmin);

                }
                catch (Exception e)
                {
                    await HelperClass.SendSMS(msg, user.PhoneNumber);
                    await HelperClass.SendSMS(msgAdmin, "08165680904,08037915777");
                    //await HelperClass.SendMail("onwukaemeka41@gmail.com", msgAdmin);
                    //await HelperClass.SendMail("judengama@gmail.com", msgAdmin);
                }
                //return RedirectToAction("VerifyAccount", new { userId = user.Id });
                return RedirectToAction("AccountInitialization", new { userId = user.Id });

            }

            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", userProgram.ProgramCourseId);
            ViewBag.userId = userId;
            return View(userProgram);
        }
        //
        [AllowAnonymous]
        public async Task<ActionResult> VerifyAccount(string userId, string accountinfo, string check)
        {
            var user = await UserManager.FindByIdAsync(userId);
            ViewBag.userid = user.Id;
            if (user.ComfirmVerifyCode == true)
            {
                return RedirectToAction("AccountInitialization", new { userId = user.Id });
            }
            ///
            if (accountinfo == "not activated")
            {
                try
                {
                    Random _random = new Random();

                    var Vnumber = _random.Next(0, 9999).ToString("D4");

                    user.VerifyCode = Vnumber;
                    await UserManager.UpdateAsync(user);
                    string callbk = "Activation Number: " + user.VerifyCode;
                    string msg = "Welcome to EXWHYZEE EDI. " + callbk;

                    await HelperClass.SendSMS(msg, user.PhoneNumber);
                    await HelperClass.SendMail(user.Email, callbk);
                }
                catch (Exception c)
                {

                }
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                ViewData["usernumber"] = user.PhoneNumber.Substring(user.PhoneNumber.Length - 4).ToLower();

            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                string a = "";
                string b = "";
                try
                {
                    a = user.Email.Substring(user.Email.Length - 10);
                }
                catch (Exception d) { }
                try
                {
                    b = user.Email.Substring(0, 4);
                }
                catch (Exception d) { }


                ViewData["usermail"] = b.ToLower() + "***" + a.ToLower();
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyAccount(string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            if (user.VerifyCode == code)
            {
                user.ComfirmVerifyCode = true;
                await UserManager.UpdateAsync(user);
                return RedirectToAction("AccountInitialization", new { userId = user.Id });
            }
            TempData["error"] = "Invalid Activation number or not the current activation number. click to Resend activation code";
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ResendCode(string userid)
        {
            var user = await UserManager.FindByIdAsync(userid);
            try
            {
                Random _random = new Random();

                var Vnumber = _random.Next(0, 9999).ToString("D4");

                user.VerifyCode = Vnumber;
                await UserManager.UpdateAsync(user);
                string callbk = "Activation Number: " + user.VerifyCode;
                string msg = "Welcome to EXWHYZEE EDI. " + callbk;

                await HelperClass.SendSMS(msg, user.PhoneNumber);
                await HelperClass.SendMail(user.Email, callbk);
                TempData["success"] = "Activation Number sent successful";
                return RedirectToAction("VerifyAccount", new { userId = user.Id });
            }
            catch (Exception c)
            {

            }
            TempData["error"] = "Unable to send Activation Number.";

            return RedirectToAction("VerifyAccount", new { userId = user.Id });

        }

        [AllowAnonymous]
        public async Task<ActionResult> AccountInitialization(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            var userprogram = await db.UserPrograms.Where(x => x.UserId == user.Id && x.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Pending).FirstOrDefaultAsync();
            var Cprogram = await db.ProgramCourses.FirstOrDefaultAsync(x => x.Id == userprogram.ProgramCourseId);

            Payment payinfo = new Payment();
            payinfo.SchoolId = user.SchoolId;
            payinfo.PaymentStatus = Models.Entities.Enum.PaymentStatus.Pending;
            payinfo.PaymentInitalizedDate = DateTime.UtcNow;
            payinfo.UserId = user.Id;
            payinfo.UserProgramId = userprogram.Id;
            payinfo.Amount = Cprogram.Amount;
            await _paymentServices.Create(payinfo);


            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Secured");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> UpdatePaid(CheckDto models)
        {
            string data = "";
            if (ModelState.IsValid)
            {

                var datamajor = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == models.UserId);

                if (datamajor != null)
                {
                    var check = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.PhoneNumber == datamajor.User.PhoneNumber);
                    if (check == null)
                    {
                        ExwhyzeeModel s = new ExwhyzeeModel();
                        s.Surname = datamajor.Surname;
                        s.Firstname = datamajor.FirstName;
                        s.Othername = datamajor.OtherName;
                        s.PhoneNumber = datamajor.User.PhoneNumber;
                        s.Paid = true;
                        s.Date = datamajor.DateRegistered;
                        db.ExwhyzeeModels.Add(s);
                        await db.SaveChangesAsync();
                        data = "User Added";
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        data = "Phone number found. kindly confirm";
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            data = "Unable to Process";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Secured");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
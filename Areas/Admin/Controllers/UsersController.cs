using ExwhyzeeEDI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using ExwhyzeeEDI.Web.Models.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ExwhyzeeEDI.Web.Models.Dtos;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity.Owin;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{//SuperAdmin
    [Authorize(Roles = "SuperAdmin,Data,Readonly,MainList,BossAdmin")]
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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


        // GET: Admin/Users
        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> Index(DateTime? DateOne, DateTime? DateTwo)
        {

            #region date
            if (DateOne != null)
            {
                DateOne = DateOne.Value.Date;
            }
            else
            {
                DateOne = null;
            }
            if (DateTwo != null)
            {
                DateTwo = DateTwo.Value.Date;
            }
            else
            {
                DateTwo = null;
            }
            if (DateOne != null || DateTwo != null)
            {
                DateTime? StartDate = DateOne.Value;
                DateTime? EndDate = DateTwo.Value;
            }
            #endregion
            IQueryable<Profile> profile = from s in db.Profiles
                                             .Include(x => x.User).Include(x => x.GetSchool)
                                            .OrderByDescending(x => x.DateRegistered)
                                          select s;
            if (DateOne != null || DateTwo != null)
            {
                var result = profile.ToList().Where(x => x.DateRegistered.Date >= DateOne).Where(x => x.DateRegistered.Date <= DateTwo).ToList();
                return View(result);
            }
            //   var profile = eawait db.Profiles.ToListAsync();
            return View(profile);
        }

        public async Task<ActionResult> IList()
        {

           
            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels
                                          select s;
            
            return View(profile);
        }

        public async Task<ActionResult> Deletex(int id)
        {
            ExwhyzeeModel slider = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.Id == id);
            db.ExwhyzeeModels.Remove(slider);
            await db.SaveChangesAsync();
            return RedirectToAction("TrainingList");
        }

        [Authorize(Roles = "SuperAdmin,Data,BossAdmin")]

        public async Task<ActionResult> SortOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Data,BossAdmin")]

        public async Task<ActionResult> SortOrder(ExwhyzeeModel data)
        {

            if (ModelState.IsValid)
            {
                var check = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.Id == data.Id);
                check.SortOrder = data.SortOrder;
                db.Entry(check).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TrainingList");
            }


            return View(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        [Authorize(Roles = "BossAdmin")]

        public async Task<ActionResult> AdminEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await db.ExwhyzeeModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BossAdmin")]

        public async Task<ActionResult> AdminEdit(ExwhyzeeModel data)
        {

            if (ModelState.IsValid)
            {
                var check = await db.ExwhyzeeModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == data.Id);
                check.Firstname = data.Firstname;
                check.Othername = data.Othername;
                check.Surname = data.Surname;
                check.BVN = data.BVN;
                check.Date = data.Date;
                check.EDI = data.EDI;
                check.EquipmentAmount = data.EquipmentAmount;
                check.Gender = data.Gender;
                check.LoanAmount = data.LoanAmount;
                check.LoanTenor = data.LoanTenor;
                check.Paid = data.Paid;
                check.PhoneNumber = data.PhoneNumber;
                check.RegistrationNumber = data.RegistrationNumber;
                check.Sector = data.Sector;
                check.SortOrder = data.SortOrder;
                check.StateOfBusinessLocation = data.StateOfBusinessLocation;
                check.SubSector = data.SubSector;
                check.UploadBy = data.UploadBy;
                check.Uploaded = data.Uploaded;
                check.UserId = data.UserId;
                check.WorkingCapital = data.WorkingCapital;


                db.Entry(check).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TrainingList");
            }


            return View(data);
        }



        ////
        ///
        ///
        ///

        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> EditTraining(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> EditTraining(ExwhyzeeModel data, int ord = 0)
        {

            if (ModelState.IsValid)
            {
                var check = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.BVN == data.BVN);
                if (check != null)
                {
                    TempData["error"] = "BVN already existing";
                    return View(data);
                }
                var user = User.Identity.Name;
                data.UploadBy = user;
                if (ord > 0)
                {
                    data.SortOrder = ord;
                }
                else
                {
                    data.SortOrder = data.Id;
                }
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TrainingList");
            }


            return View(data);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ValidateAmount(string Capital, string Machine, string Total)
        {
            string data = "";
            string data2 = "";

            var c = decimal.Parse(Regex.Replace(Capital, @"[^\d.]", ""));
            // var m = decimal.Parse(Regex.Replace(Machine, @"[^\d.]", ""));
            var t = decimal.Parse(Regex.Replace(Total, @"[^\d.]", ""));

            var result = t - c;
            //if(result == t)
            //{
            //    data2 = "correct. You can now submit the form";
            //}
            //else
            //{
            //    data2 = "wrong. You cant submit the form";
            //}
            var ert = Convert.ToDecimal(result).ToString("#,##0.00");
            data = result.ToString();
            String.Format("{0:C2}", result);
            string outr = "N" + ert.ToString();
            return Json(outr, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "BossAdmin,SuperAdmin")]
        public async Task<ActionResult> UpdatePaid(string UserId)
        {
            string data = "";
            if (ModelState.IsValid)
            {

                var datamajor = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == UserId);

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
                        s.RegistrationNumber = datamajor.RegistrationNumber;
                        var school = db.Schools.FirstOrDefault(x => x.Id == datamajor.User.SchoolId);
                        s.EDI = school.SchoolName;
                        s.Paid = true;
                        s.UserId = datamajor.UserId;
                        s.Date = datamajor.DateRegistered;

                        var user = User.Identity.Name;
                        s.UploadBy = user;
                        s.SortOrder = 1004;
                        db.ExwhyzeeModels.Add(s);
                        await db.SaveChangesAsync();
                        data = "User Added";
                        // return Json(data, JsonRequestBehavior.AllowGet);
                        return RedirectToAction("EditTraining", new { id = s.Id });
                    }
                    else
                    {
                        data = "This number has already been added. You cannot add it again";
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            data = "Unable to Process";
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SuperAdmin,Data,BossAdmin")]
        public async Task<ActionResult> RC(string UserId)
        {

            var datamajor = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == UserId);

            if (datamajor != null)
            {
                if (datamajor.NeedCertificate == true)
                {
                    datamajor.NeedCertificate = false;
                }
                else
                {
                    datamajor.NeedCertificate = true;
                }

                db.Entry(datamajor).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return Json(datamajor.NeedCertificate, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> BP(string UserId)
        {

            var datamajor = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == UserId);

            if (datamajor != null)
            {
                if (datamajor.NeedBusinessPlan == true)
                {
                    datamajor.NeedBusinessPlan = false;
                }
                else
                {
                    datamajor.NeedBusinessPlan = true;
                }

                db.Entry(datamajor).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return Json(datamajor.NeedCertificate, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public async Task<ActionResult> BVNCheck(string phone)
        {
            try
            {
                string apiurl = $"https://api.flutterwave.com/v3/kyc/bvns/" + phone;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                var client = new RestClient(apiurl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "FLWSECK-3ebc176be7413ec684592804c5cd98b7-X");


                IRestResponse response = client.Execute(request);
                var contents = response.Content.ToString();
                //var json = await response.Content.ReadAsStringAsync();
                string outp = "";
                var mainresponse = JsonConvert.DeserializeObject<BVN_Response>(contents);
                if (mainresponse.status == "success")
                {
                    outp = "BVN: " + mainresponse.data.bvn + "- Name: " + mainresponse.data.first_name + " " + mainresponse.data.middle_name + " " + mainresponse.data.last_name;
                }
                else
                {
                    outp = "Name not found";
                }

                return Json(outp, JsonRequestBehavior.AllowGet);

            }
            catch (Exception g)
            {
                return Json("not found", JsonRequestBehavior.AllowGet);

            }
        }

        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> TrainingDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExwhyzeeModel data = await db.ExwhyzeeModels.FirstOrDefaultAsync(x => x.Id == id);
            return View(data);
        }

        //[Authorize(Roles = "BossAdmin,SuperAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult> Validatelistsortorder()
        {

            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels
                                                .Where(x => x.EDI == "EXWHYZEE TECHNOLOGIES LTD")
                                            .OrderByDescending(x => x.Id)
                                                select s;
            var p = profile.ToList();
            foreach (var i in profile)
            {
                i.EDI = "EXWHYZEE TECHNOLOGIES";
                db.Entry(i).State = EntityState.Modified;

            }
            await db.SaveChangesAsync();
            //   var profile = eawait db.Profiles.ToListAsync();
            return View(profile);
        }
        [Authorize(Roles = "BossAdmin")]
        public async Task<ActionResult> TrainingList()
        {

            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels

                                            .OrderByDescending(x => x.Id)
                                                select s;

            //   var profile = eawait db.Profiles.ToListAsync();
            return View(profile);
        }
        [Authorize(Roles = "OwerriBranch,UmuahiaBranch,SuperAdmin")]
        public async Task<ActionResult> MainList()
        {

            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels

                                            .OrderBy(x => x.SortOrder)
                                                select s;

            return View(profile);
        }

        public async Task<ActionResult> Validate()
        {

            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels

                                            .OrderBy(x => x.SortOrder)
                                                select s;

           
            foreach (var i in profile.Where(x=>String.IsNullOrEmpty(x.CertificateNumber)).ToList())
            {
                var h = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == i.UserId);
                i.CertificateNumber = h.CertificateId;
                db.Entry(i).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            
            return View(profile);
        }

        [Authorize(Roles = "Readonly,MainList,BossAdmin")]
        public async Task<ActionResult> TrainingResponse()
        {

            IQueryable<ExwhyzeeModel> profile = from s in db.ExwhyzeeModels

                                            .OrderBy(x => x.SortOrder)
                                                select s;

            return View(profile);
        }


        public async Task<ActionResult> Certified()
        {
            var profile = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).Where(x => x.CollectedCertificate == true).OrderByDescending(x => x.DateRegistered).ToListAsync();
            return View(profile);
        }
        public async Task<ActionResult> Enrolled()
        {
            var profile = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).Where(x => x.UserStatus == Models.Entities.Enum.UserStatus.Started).OrderByDescending(x => x.DateRegistered).ToListAsync();
            return View(profile);
        }
        public async Task<ActionResult> CompletedTraining()
        {
            var profile = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).Where(x => x.UserStatus == Models.Entities.Enum.UserStatus.Completed).OrderByDescending(x => x.DateRegistered).ToListAsync();
            return View(profile);
        }
        public async Task<ActionResult> PendingTraining()
        {
            var profile = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).Where(x => x.UserStatus == Models.Entities.Enum.UserStatus.Pending || x.UserStatus == Models.Entities.Enum.UserStatus.None).OrderByDescending(x => x.DateRegistered).ToListAsync();
            return View(profile);
        }

        public async Task<ActionResult> RegisteredProgram(string uId)
        {
            var uprogram = await db.UserPrograms.Include(x => x.GetSchool).Include(x => x.ProgramCourse).Where(x => x.UserId == uId).ToListAsync();
            return View(uprogram);
        }

        public async Task<ActionResult> UpdateProgram(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgram data = await db.UserPrograms.FindAsync(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProgram(UserProgram data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("RegisteredProgram", new { uId = data.UserId });
            }
            return View(data);
        }

        public async Task<ActionResult> UpdateUserData(string UserId)
        {

            var data = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }



        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserData(Profile data)
        {
            if (ModelState.IsValid)
            {
                var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == data.UserId);
                datamajor.UserId = data.UserId;
                datamajor.RecommendationNote = data.RecommendationNote;
                datamajor.CertificateDate = data.CertificateDate;
                datamajor.DateEnrolledForTraining = DateTime.UtcNow.AddHours(1);
                db.Entry(datamajor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["success"] = "Updated Successfully";
                return RedirectToAction("Details", new { id = data.UserId });
            }
            TempData["error"] = "Error, Try Again";
            return View(data);
        }

        public async Task<ActionResult> ProcessCertificate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
            Random num = new Random();

            // Create new string from the reordered char array
            string rand = new string(date1.ToCharArray().
                            OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
            string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

            var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
            datamajor.CertificateId = "EDI/" + rand + date2;

            db.Entry(datamajor).State = EntityState.Modified;
            await db.SaveChangesAsync();
            TempData["success"] = "Updated Successfully";
            return RedirectToAction("Details", new { id = id });
        }



        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profile data = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).FirstOrDefaultAsync(x => x.UserId == id);


            if (data == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(data.CertificateId))
            {
                string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                Random num = new Random();

                // Create new string from the reordered char array
                string rand = new string(date1.ToCharArray().
                                OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
                string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                //   var datamajor = await db.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
                data.CertificateId = "EDI/" + rand + date2;

                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return View(data);
        }
        [AllowAnonymous]
        public ActionResult ABPCertificate()
        {
            return RedirectToAction("Certificate");
        }
        [AllowAnonymous]
        public ActionResult Certificate()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<ActionResult> Certverify(string certrefno)
        {
            if (certrefno == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (certrefno.Contains("EXW/VON/ABP/"))
            {
                return RedirectToAction("Verify", "ABP", new { xyzid = certrefno, area = "" });
            }
            Profile data = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).FirstOrDefaultAsync(x => x.CertificateId == certrefno);
            if (data == null)
            {
                TempData["error"] = "Incorrect or Invalid Ref Number";
                return RedirectToAction("Certificate");
            }
            return View(data);
        }
        private byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }


        public async Task<ActionResult> UpdateCertStatus(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile data = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).FirstOrDefaultAsync(x => x.UserId == UserId);


            if (data == null)
            {
                return HttpNotFound();
            }
            if (data.CollectedCertificate == true)
            {
                data.CollectedCertificate = false;

            }
            else
            {
                data.CollectedCertificate = true;
            }
            db.Entry(data).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = UserId });
        }

        [AllowAnonymous]
        public async Task<ActionResult> PrintCertificate(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile data = await db.Profiles.Include(x => x.User).Include(x => x.GetSchool).FirstOrDefaultAsync(x => x.UserId == UserId);
            try
            {
                if (String.IsNullOrEmpty(data.CertificateDate))
                {
                    TempData["errror"] = "Update certitficate date";
                    return RedirectToAction("Details", new { id = UserId });
                }

                data.CertificatePrinted = Models.Entities.Enum.EdiStatus.Yes;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            catch (Exception c) { }
            Zen.Barcode.CodeQrBarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            string userinfo = "";
            try

            {
                userinfo = data.Surname + " with Ref. " + data.CertificateId + " is Valid. follow link to verify. http://edi.exwhyzee.ng/Admin/Users/Certificate";
                var img = barcode.Draw(userinfo, 100);
                byte[] imgBytes = turnImageToByteArray(img);
                string imgString = Convert.ToBase64String(imgBytes);
                ViewBag.img = imgBytes;
            }
            catch (Exception c)
            {
                userinfo = data.FirstName + " with Ref. " + data.CertificateId + " is Valid. follow link to verify. http://edi.exwhyzee.ng/Admin/Users/Certificate";
                var img = barcode.Draw(userinfo, 100);
                byte[] imgBytes = turnImageToByteArray(img);
                string imgString = Convert.ToBase64String(imgBytes);
                ViewBag.img = imgBytes;
            }





            var item = await db.CertificateInfos.FirstOrDefaultAsync();
            ViewBag.item = item;

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }


        //anchore borr...
        public ActionResult CreateABP()
        {
            return View();
        }

        // POST: Admin/BusinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateABP(ABP data)
        {
            if (ModelState.IsValid)
            {
                data.DateTime = DateTime.UtcNow.AddHours(1);
                db.ABPs.Add(data);
                await db.SaveChangesAsync();
                var fetch = await db.ABPs.FirstOrDefaultAsync(x => x.Id == data.Id);
                fetch.RegNumber = "EXW/VON/ABP/" + data.Id.ToString("00000");
                db.Entry(fetch).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexABP");
            }

            return View(data);
        }

        public async Task<ActionResult> IndexABP()
        {
            var abp = await db.ABPs.OrderByDescending(x => x.DateTime).ToListAsync();
            return View(abp);
        }

        public async Task<ActionResult> PrintCertificateABP(int id)
        {

            ABP data = await db.ABPs.FirstOrDefaultAsync(x => x.Id == id);

            Zen.Barcode.CodeQrBarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            string userinfo = "";
            try

            {
                userinfo = data.FullName + " with Ref. " + data.RegNumber + " is Valid. http://edi.exwhyzee.ng/Admin/Users/ABPCertificate";
                var img = barcode.Draw(userinfo, 100);
                byte[] imgBytes = turnImageToByteArray(img);
                string imgString = Convert.ToBase64String(imgBytes);
                ViewBag.img = imgBytes;
            }
            catch (Exception c)
            {
                userinfo = data.FullName + " with Ref. " + data.RegNumber + " is Valid. http://edi.exwhyzee.ng/Admin/Users/ABPCertificate";
                var img = barcode.Draw(userinfo, 100);
                byte[] imgBytes = turnImageToByteArray(img);
                string imgString = Convert.ToBase64String(imgBytes);
                ViewBag.img = imgBytes;
            }


            return View(data);
        }

        // GET: Admin/Authors/Edit/5
        public async Task<ActionResult> EditABP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await db.ABPs.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditABP(ABP data)
        {

            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexABP");
            }


            return View(data);
        }



        //EDIT
        [Authorize(Roles = "SuperAdmin,Data")]

        public async Task<ActionResult> EditProfile(string uid)
        {

            var author = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == uid);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Data")]

        public async Task<ActionResult> EditProfile(Profile data, string Email, string Phone)
        {

            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(Email))
                {
                    var check = await db.Users.FirstOrDefaultAsync(x => x.Email == Email);
                    if (check != null)
                    {
                        ViewBag.error = "Email Already Taken";
                    }
                    var checki = await db.Users.FirstOrDefaultAsync(x => x.Id == data.UserId);
                    checki.Email = Email;
                    db.Entry(checki).State = EntityState.Modified;
                }
                if (!String.IsNullOrEmpty(Phone))
                {
                    var checks = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == Phone);
                    if (checks != null)
                    {
                        ViewBag.error = "Phone Already Taken";
                    }
                    var checkis = await db.Users.FirstOrDefaultAsync(x => x.Id == data.UserId);
                    checkis.PhoneNumber = Phone;
                    db.Entry(checkis).State = EntityState.Modified;
                }
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(data);
        }


        public async Task<ActionResult> BatchRegEx()
        {

            return View();
        }

        // POST: Admin/UserManagers/BatchReg
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public async Task<ActionResult> BatchRegEx(ExwhyzeeModel model, HttpPostedFileBase excelfile)
        {
            string succed;
            string ee = "";
            string path = "";
            string nameswitheerror = "";
            int sn = 1;
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                TempData["error"] = "Please select an excel file";
                return RedirectToAction("BatchReg");

            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    path = Server.MapPath("~/ExcelUpload/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        TempData["error"] = "Excel file with same name exist try renaming the excel file";
                        return RedirectToAction("BatchReg");
                    }

                    //System.IO.File.Delete(path);

                    excelfile.SaveAs(path);

                    //Read Data From Excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    try
                    {
                        for (int row = 3; row <= range.Rows.Count; row++)
                        {
                            try
                            {
                                string userCheck = ((Excel.Range)range.Cells[row, 1]).Text;


                                string a1 = ((Excel.Range)range.Cells[row, 2]).Text;
                                string bvn = ((Excel.Range)range.Cells[row, 3]).Text;
                                string pho = ((Excel.Range)range.Cells[row, 4]).Text;
                                string statofbiz = ((Excel.Range)range.Cells[row, 5]).Text;
                                string arw = ((Excel.Range)range.Cells[row, 6]).Text;
                                string gender = ((Excel.Range)range.Cells[row, 7]).Text;
                                string edi = ((Excel.Range)range.Cells[row, 8]).Text;
                                string sec = ((Excel.Range)range.Cells[row, 9]).Text;
                                string sub = ((Excel.Range)range.Cells[row, 10]).Text;
                                string eqp = ((Excel.Range)range.Cells[row, 11]).Text;
                                string work = ((Excel.Range)range.Cells[row, 12]).Text;
                                string ltenor = ((Excel.Range)range.Cells[row, 13]).Text;
                                //

                                Random _random = new Random();
                                var Vnumber = _random.Next(0, 999999999).ToString("D6");
                                string emaild = "";
                                emaild = "asdfwewe22" + Vnumber + "o@xydz.com";

                                // var Vnumber = _random.Next(0, 9999).ToString("D4");
                                var user = new ApplicationUser { UserName = emaild, Email = emaild, PhoneNumber = pho, VerifyCode = Vnumber, SchoolId = 5 };
                                var result = await UserManager.CreateAsync(user, "Nirsal@123");
                                string[] sio = a1.Split(' ');
                                if (result.Succeeded)
                                {


                                    await UserManager.AddToRoleAsync(user.Id, "Participant");
                                }
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
                                await db.SaveChangesAsync();

                                ///

                                string date1 = DateTime.UtcNow.AddHours(1).ToString("yyyyMM");
                                Random num = new Random();

                                // Create new string from the reordered char array
                                string rand = new string(date1.ToCharArray().
                                                OrderBy(r => (num.Next(2) % 2) == 0).ToArray());
                                string date2 = DateTime.UtcNow.AddHours(1).ToString("HHmmMMddss");

                                var datamajor = await db.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == profile.Id);
                                datamajor.CertificateId = "EDI/" + rand + date2;
                                string lastnumberid = datamajor.Id.ToString("D4");
                                string number = "EDI/XYZ/2019/" + datamajor.Id;
                                datamajor.RegistrationNumber = number;
                                db.Entry(datamajor).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                //
                                ExwhyzeeModel s = new ExwhyzeeModel();

                                s.Surname = datamajor.Surname;
                                s.Firstname = datamajor.FirstName;
                                s.Othername = datamajor.OtherName;
                                s.PhoneNumber = datamajor.User.PhoneNumber;
                                s.RegistrationNumber = datamajor.RegistrationNumber;
                                var school = db.Schools.FirstOrDefault(x => x.Id == datamajor.User.SchoolId);
                                s.EDI = school.SchoolName;
                                s.Paid = true;
                                s.UserId = datamajor.UserId;
                                s.Date = datamajor.DateRegistered;
                                s.BVN = bvn;
                                s.EquipmentAmount = "N" + eqp;
                                s.WorkingCapital = "N" + work;
                                s.LoanTenor = ltenor;
                                s.StateOfBusinessLocation = statofbiz;
                                s.Gender = gender;
                                s.Sector = sec;
                                s.SubSector = sub;

                                s.UploadBy = "excel";
                                s.SortOrder = 2004;
                                db.ExwhyzeeModels.Add(s);
                                await db.SaveChangesAsync();
                            }
                            catch (Exception k)
                            {

                            }
                        }
                    }

                    catch (Exception e)
                    {
                        ee = e.ToString();
                    }
                    TempData["msg"] = "Uploaded successfully.";
                    TempData["batcherror"] = nameswitheerror;
                    return RedirectToAction("BatchReg");


                }
                else
                {

                    TempData["error"] = "file type incorrect";

                }

                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["error1"] = ee;

                return View();

            }
        }




    }



}
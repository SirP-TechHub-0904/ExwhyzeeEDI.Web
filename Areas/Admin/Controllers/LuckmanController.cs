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
using System.Globalization;
using System.Data.Entity.SqlServer;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class LuckmanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Users
        public async Task<ActionResult> Index()
        {
         
            IQueryable<Profile> profile = from s in db.Profiles
                                          .Include(x=>x.User)
                                          .Where(x=>x.ProgramBy == "luckman")
                                               .OrderByDescending(x => x.DateRegistered)
                                          select s;

            ViewBag.PaidForTraining = profile.Where(x => x.PaidForTraining == true).Count();
            ViewBag.PaidForTrainingn = profile.Where(x => x.PaidForTraining == false).Count();
            ViewBag.Attendance = profile.Where(x => x.Attendance == Models.Entities.Enum.EdiStatus.Yes).Count();
            ViewBag.Attendancen = profile.Where(x => x.Attendance == Models.Entities.Enum.EdiStatus.No).Count();
            ViewBag.CollectedCertificate = profile.Where(x => x.CollectedCertificate == true).Count();
            ViewBag.CollectedCertificaten = profile.Where(x => x.CollectedCertificate == false).Count();
            ViewBag.CertificatePrinted = profile.Where(x => x.CertificatePrinted == Models.Entities.Enum.EdiStatus.Yes).Count();
            ViewBag.CertificatePrintedn = profile.Where(x => x.CertificatePrinted == Models.Entities.Enum.EdiStatus.No).Count();
            ViewBag.NirsalPayment = profile.Where(x => x.NirsalPayment == Models.Entities.Enum.EdiStatus.Yes).Count();
            ViewBag.NirsalPaymentn = profile.Where(x => x.NirsalPayment == Models.Entities.Enum.EdiStatus.No).Count();
            ViewBag.BusinessPlanUpload = profile.Where(x => x.BusinessPlanUpload == Models.Entities.Enum.EdiStatus.Yes).Count();
            ViewBag.BusinessPlanUploadn = profile.Where(x => x.BusinessPlanUpload == Models.Entities.Enum.EdiStatus.No).Count();
            ViewBag.FormStatus = profile.Where(x => x.FormStatus == Models.Entities.Enum.FormStatus.Seen).Count();
            ViewBag.FormStatusn = profile.Where(x => x.FormStatus == Models.Entities.Enum.FormStatus.NotSeen).Count();
            /*
             PaidForTraining
                Attendance 	NO
                CollectedCertificate
                CertificatePrinted 	NO
                NirsalPayment 	NO
                BusinessPlanUpload 	NO
                FormStatus 	NOT SE*/
            return View(profile);
        }

        public async Task<ActionResult> IndexMain()
        {

            IQueryable<Profile> profile = from s in db.Profiles

                                          
                                               .OrderByDescending(x => x.DateRegistered).Take(319)
                                          select s;
            var fd = profile.Count();
            foreach(var i in profile)
            {
                i.ProgramBy = "luckman";
               // db.Entry(i).State = EntityState.Modified;
                
            }
            await db.SaveChangesAsync();
            return View(profile);
        }

        /// <summary>
        /// 
        /*
         PaidForTraining 	
Attendance 	NO
CollectedCertificate 	
CertificatePrinted 	NO
NirsalPayment 	NO
BusinessPlanUpload 	NO
FormStatus 	NOT SEEN
         */
        //
        [AllowAnonymous]
        public async Task<ActionResult> FormStatus(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.FormStatus == Models.Entities.Enum.FormStatus.Seen)
            {
                io.FormStatus = Models.Entities.Enum.FormStatus.NotSeen;
                sos = "NO";
            }
            else
            {
                io.FormStatus = Models.Entities.Enum.FormStatus.Seen;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public async Task<ActionResult> BusinessPlanUpload(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.BusinessPlanUpload == Models.Entities.Enum.EdiStatus.Yes)
            {
                io.BusinessPlanUpload = Models.Entities.Enum.EdiStatus.No;
                sos = "NO";
            }
            else
            {
                io.BusinessPlanUpload = Models.Entities.Enum.EdiStatus.Yes;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public async Task<ActionResult> NirsalPayment(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.NirsalPayment == Models.Entities.Enum.EdiStatus.Yes)
            {
                io.NirsalPayment = Models.Entities.Enum.EdiStatus.No;
                sos = "NO";
            }
            else
            {
                io.NirsalPayment = Models.Entities.Enum.EdiStatus.Yes;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public async Task<ActionResult> CertificatePrinted(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.CertificatePrinted == Models.Entities.Enum.EdiStatus.Yes)
            {
                io.CertificatePrinted = Models.Entities.Enum.EdiStatus.No;
                sos = "NO";
            }
            else
            {
                io.CertificatePrinted = Models.Entities.Enum.EdiStatus.Yes;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public async Task<ActionResult> CollectedCertificate(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.CollectedCertificate == true)
            {
                io.CollectedCertificate = false;
                sos = "NO";
            }
            else
            {
                io.CollectedCertificate = true;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public async Task<ActionResult> Attendance(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if (io.Attendance == Models.Entities.Enum.EdiStatus.Yes)
            {
                io.Attendance = Models.Entities.Enum.EdiStatus.No;
                sos = "NO";
            }
            else
            {
                io.Attendance = Models.Entities.Enum.EdiStatus.Yes;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(sos, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public async Task<ActionResult> PaidForTraining(string Id)
        {
            string sos = "";
            var io = db.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == Id);
            if(io.PaidForTraining == true)
            {
                io.PaidForTraining = false;
                sos = "NO";
            }
            else
            {
                io.PaidForTraining = true;
                sos = "YES";
            }
            db.Entry(io).State = EntityState.Modified;
            await db.SaveChangesAsync();
            
            return Json(sos, JsonRequestBehavior.AllowGet);

        }

        /// <param name="id"></param>
        /// <returns></returns>

        // GET: Admin/Luckman/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Luckman/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Luckman/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Luckman/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Luckman/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Luckman/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Luckman/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

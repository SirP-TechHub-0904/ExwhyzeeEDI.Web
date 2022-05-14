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
using System.IO;
using Microsoft.AspNet.Identity;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class BusinessPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/BusinessPlans
        public async Task<ActionResult> Index()
        {
            return View(await db.BusinessPlans.ToListAsync());
        }
        // GET: EDI/BusinessPlans/Create
        public ActionResult UploadBusinessPlan()
        {

            return View();
        }

        // POST: EDI/BusinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadBusinessPlan(BusinessPlan businessPlan, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {


                    string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                    string name = date1 + "-" + upload.FileName;
                    string fileName = Path.GetFileName(name);
                    businessPlan.BusinessPlanUrl = fileName;
                    fileName = Path.Combine(Server.MapPath("~/BusinessPlan/"), fileName);
                    upload.SaveAs(fileName);

                }
                var user = User.Identity.GetUserId();
                var userinfo = await db.Users.FirstOrDefaultAsync(x => x.Id == user);
                businessPlan.Date = DateTime.UtcNow;
                businessPlan.UserId = userinfo.Id;
                db.BusinessPlans.Add(businessPlan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(businessPlan);
        }

        // GET: Admin/BusinessPlans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessPlan businessPlan = await db.BusinessPlans.FindAsync(id);
            if (businessPlan == null)
            {
                return HttpNotFound();
            }
            return View(businessPlan);
        }

        // GET: Admin/BusinessPlans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BusinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BusinessName,BusinessProfile,BusinessPlanUrl,Date")] BusinessPlan businessPlan)
        {
            if (ModelState.IsValid)
            {
                db.BusinessPlans.Add(businessPlan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(businessPlan);
        }

        // GET: Admin/BusinessPlans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessPlan businessPlan = await db.BusinessPlans.FindAsync(id);
            if (businessPlan == null)
            {
                return HttpNotFound();
            }
            return View(businessPlan);
        }

        // POST: Admin/BusinessPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BusinessName,BusinessProfile,BusinessPlanUrl,Date")] BusinessPlan businessPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessPlan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(businessPlan);
        }

        // GET: Admin/BusinessPlans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessPlan businessPlan = await db.BusinessPlans.FindAsync(id);
            if (businessPlan == null)
            {
                return HttpNotFound();
            }
            return View(businessPlan);
        }

        // POST: Admin/BusinessPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BusinessPlan businessPlan = await db.BusinessPlans.FindAsync(id);
            db.BusinessPlans.Remove(businessPlan);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class ApplicantCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ApplicantCategories
        public ActionResult Index()
        {
            return View(db.ApplicantCategorys.ToList());
        }

        // GET: Admin/ApplicantCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantCategory applicantCategory = db.ApplicantCategorys.Find(id);
            if (applicantCategory == null)
            {
                return HttpNotFound();
            }
            return View(applicantCategory);
        }

        // GET: Admin/ApplicantCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ApplicantCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateCreated,Description")] ApplicantCategory applicantCategory)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantCategorys.Add(applicantCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicantCategory);
        }

        // GET: Admin/ApplicantCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantCategory applicantCategory = db.ApplicantCategorys.Find(id);
            if (applicantCategory == null)
            {
                return HttpNotFound();
            }
            return View(applicantCategory);
        }

        // POST: Admin/ApplicantCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateCreated,Description")] ApplicantCategory applicantCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicantCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicantCategory);
        }

        // GET: Admin/ApplicantCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantCategory applicantCategory = db.ApplicantCategorys.Find(id);
            if (applicantCategory == null)
            {
                return HttpNotFound();
            }
            return View(applicantCategory);
        }

        // POST: Admin/ApplicantCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicantCategory applicantCategory = db.ApplicantCategorys.Find(id);
            db.ApplicantCategorys.Remove(applicantCategory);
            db.SaveChanges();
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

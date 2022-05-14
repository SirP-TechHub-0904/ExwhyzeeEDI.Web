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

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class CertificateInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/CertificateInfoes
        public async Task<ActionResult> Index()
        {
            return View(await db.CertificateInfos.ToListAsync());
        }

        // GET: Admin/CertificateInfoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateInfo certificateInfo = await db.CertificateInfos.FindAsync(id);
            if (certificateInfo == null)
            {
                return HttpNotFound();
            }
            return View(certificateInfo);
        }

        // GET: Admin/CertificateInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CertificateInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CertificateInfo certificateInfo)
        {
            if (ModelState.IsValid)
            {
                db.CertificateInfos.Add(certificateInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(certificateInfo);
        }

        // GET: Admin/CertificateInfoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateInfo certificateInfo = await db.CertificateInfos.FindAsync(id);
            if (certificateInfo == null)
            {
                return HttpNotFound();
            }
            return View(certificateInfo);
        }

        // POST: Admin/CertificateInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Signature,SignatureName")] CertificateInfo certificateInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificateInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(certificateInfo);
        }

        // GET: Admin/CertificateInfoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificateInfo certificateInfo = await db.CertificateInfos.FindAsync(id);
            if (certificateInfo == null)
            {
                return HttpNotFound();
            }
            return View(certificateInfo);
        }

        // POST: Admin/CertificateInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CertificateInfo certificateInfo = await db.CertificateInfos.FindAsync(id);
            db.CertificateInfos.Remove(certificateInfo);
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
